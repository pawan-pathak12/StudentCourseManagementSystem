using Microsoft.Extensions.Logging;
using StudentCourseManagement.API.DTOs.FInancialModule.FeeAssessments;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Constants;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using System.Transactions;

namespace StudentCourseManagement.Business.Services.FinancialModule
{
    public class FeeAssessmentService : IFeeAssessmentService
    {
        private readonly IFeeAssessmentRepository _feeAssessmentRepository;
        private readonly ILogger<FeeAssessmentService> _logger;
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IFeeTemplateRepository _feeTemplateRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public FeeAssessmentService(IFeeAssessmentRepository feeAssessmentRepository, ILogger<FeeAssessmentService> logger, ICourseRepository courseRepository,
            IEnrollmentRepository enrollmentRepository, IFeeTemplateRepository feeTemplateRepository, IInvoiceRepository invoiceRepository)
        {
            this._feeAssessmentRepository = feeAssessmentRepository;
            this._logger = logger;
            this._courseRepository = courseRepository;
            this._enrollmentRepository = enrollmentRepository;
            this._feeTemplateRepository = feeTemplateRepository;
            this._invoiceRepository = invoiceRepository;
        }

        #region CURD Operations 
        public async Task<bool> CreateAsync(FeeAssessment feeAssessment)
        {
            var course = await _courseRepository.GetByIdAsync(feeAssessment.CourseId);
            if (course == null)
            {
                return false;
            }
            var enrollment = await _enrollmentRepository.GetByIdAsync(feeAssessment.EnrollmentId);
            if (enrollment == null)
            {
                return false;
            }
            var feeTemplate = await _feeTemplateRepository.GetByIdAsync(feeAssessment.FeeTemplateId);
            if (feeTemplate == null)
            {
                return false;
            }
            feeAssessment.IsActive = true;
            feeAssessment.FeeAssessmentStatus = AssessmentStatus.Assessed;
            await _feeAssessmentRepository.AddAsync(feeAssessment);
            return true;
        }

        public async Task<IEnumerable<FeeAssessment>> GetAllAsync()
        {
            return await _feeAssessmentRepository.GetAllAsync();
        }

        public async Task<FeeAssessment?> GetByIdAsync(int feeAssessmentId)
        {
            return await _feeAssessmentRepository.GetByIdAsync(feeAssessmentId);
        }

        public async Task<bool> UpdateAsync(int feeAssessmentId, FeeAssessment feeAssessment)
        {
            var assessment = await _feeAssessmentRepository.GetByIdAsync(feeAssessmentId);
            if (assessment == null)
            {
                _logger.LogWarning($"Update Failed : FeeAssessent with Id {feeAssessmentId} not found");
                return false;
            }

            if (feeAssessmentId != feeAssessment.FeeAssessmentId)
            {
                _logger.LogWarning($"Id mismatched");
                return false;
            }
            return await _feeAssessmentRepository.UpdateAsync(feeAssessmentId, feeAssessment);
        }

        public async Task<bool> DeleteAsync(int feeAssessmentId)
        {
            var assessment = await _feeAssessmentRepository.GetByIdAsync(feeAssessmentId);
            if (assessment == null)
            {
                _logger.LogWarning($"Delete failed : FeeAssessent with Id {feeAssessmentId} not found");
                return false;
            }
            return await _feeAssessmentRepository.DeleteAsync(feeAssessmentId);
        }

        #endregion

        #region Phase -3 Automated FeeeAssessment 
        public async Task<(bool success, string? ErrorMessage)> AssessFee(int enrollmentId)
        {
            #region Business Logic Check

            // 1. Enrollment must exist
            var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);
            if (enrollment == null)
            {
                _logger.LogWarning("FeeAssessment creation failed: Enrollment with Id {EnrollmentId} not found.", enrollmentId);
                return (false, $"Enrollment with Id {enrollmentId} not found.");
            }

            // 2. Enrollment status must be comfirmed
            if (enrollment.EnrollmentStatus == EnrollmentStatus.Cancelled
                || enrollment.EnrollmentStatus == EnrollmentStatus.Pending
                || enrollment.EnrollmentStatus == EnrollmentStatus.Failed)
            {
                _logger.LogWarning("FeeAssessment creation failed: Enrollment Id {EnrollmentId} has invalid status {Status}.",
                    enrollmentId, enrollment.EnrollmentStatus);
                return (false, $"Enrollment status must be Confirmed. Current status: {enrollment.EnrollmentStatus}");
            }

            // 3. There shouldnt be an existing FeeAssessment
            var feeAssessmentExists = await _feeAssessmentRepository.ExistsByEnrollmentIdAsync(enrollmentId);
            if (feeAssessmentExists)
            {
                _logger.LogWarning("FeeAssessment creation failed: Enrollment Id {EnrollmentId} already has an existing FeeAssessment.",
                    enrollmentId);
                return (false, $"Their is already feeAssessment for Enrollment Id {enrollmentId} ");
            }

            // 4. FeeTemplate must exist for the course
            var feeTemplate = await _feeTemplateRepository.GetActiveByCourseId(enrollment.CourseId);
            if (feeTemplate == null)
            {
                _logger.LogWarning("FeeAssessment creation failed: No active FeeTemplate found for Course Id {CourseId}.",
                    enrollment.CourseId);
                return (false, $"No active FeeTemplate found for Course Id {enrollment.CourseId}");
            }

            // 5. FeeTemplate must be active
            if (!feeTemplate.IsActive)
            {
                _logger.LogWarning("FeeAssessment creation failed: FeeTemplate Id {FeeTemplateId} for Course Id {CourseId} is inactive.",
                    feeTemplate.FeeTemplateId, enrollment.CourseId);
                return (false, $"FeeTemplate Id {feeTemplate.FeeTemplateId}for Course Id {{CourseId}} is inactive.");
            }

            #endregion

            // 6. Calculate fee 

            decimal amount = 0;
            var course = await _courseRepository.GetByIdAsync(enrollment.CourseId);

            if (feeTemplate.CalculationType == CalculationType.FlatAmount)
            {
                amount = feeTemplate.Amount;
            }
            if (feeTemplate.CalculationType == CalculationType.RatePerCredit)
            {
                amount = feeTemplate.RatePerCredit * course.Credits;
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            {
                // 7. create feeAssessment first --left to calculate amount 
                var feeAssessment = new FeeAssessment
                {
                    EnrollmentId = enrollmentId,
                    CourseId = enrollment.CourseId,
                    FeeTemplateId = feeTemplate.FeeTemplateId,
                    Amount = amount,
                    DueDate = DateTimeOffset.UtcNow.AddDays(FinancialConstants.DUE_DATE_DAYS),
                    FeeAssessmentStatus = AssessmentStatus.Assessed,
                    IsActive = true,
                    AssessmentDate = DateTimeOffset.UtcNow,
                    PaidDate = null,
                    LateFeeAmount = null,
                    LateFeeAppliedDate = null
                };

                var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

                // 8. update enrollment after feeAssessment 
                enrollment.FeeAssessmentDate = DateTimeOffset.UtcNow;
                await _enrollmentRepository.UpdateFeeAssessedDateAsync(enrollmentId);

                // 9. create invoice after feeAssessment 
                var invoice = new Invoice
                {
                    InvoiceNumber = await _invoiceRepository.GenerateInvoiceNumberAsync(),
                    StudentId = enrollment.StudentId,
                    CourseId = enrollment.CourseId,
                    FeeAssessmentId = feeAssessmentId,
                    IsActive = true,
                    TotalAmount = feeAssessment.Amount,
                    AmountPaid = 0,
                    BalanceDue = feeAssessment.Amount,
                    DueDate = DateTimeOffset.UtcNow.AddDays(FinancialConstants.DUE_DATE_DAYS),
                    InvoiceStatus = InvoiceStatus.Issued,
                    CreatedAt = DateTimeOffset.UtcNow,
                    IssuedAt = DateTimeOffset.UtcNow,
                    LateFeeApplied = false,
                    Discount = 0
                };

                var invoiceId = await _invoiceRepository.AddAsync(invoice);

                scope.Complete();
            }
            return (true, null);
        }

        public async Task<FeeAssessmentResultDto?> GetFeeAssessmentDetailsByEnrollmentIdAsync(int enrollmentId)
        {
            //get feeassessmnet from enrollment Id 
            var feeAssessment = await _feeAssessmentRepository.GetByEnrolmentIdAsync(enrollmentId);
            if (feeAssessment == null)
            {
                return null;
            }

            //get invoice from feeAssessment Id 
            var invoice = await _invoiceRepository.GetByFeeAssessmentIdAsync(feeAssessment.FeeAssessmentId);
            if (invoice == null)
            {
                return null;
            }

            //get enrollment 
            var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);
            if (enrollment == null)
            {
                return null;
            }
            //get feeTemplate from enroll course ID 
            var feeTemplate = await _feeTemplateRepository.GetActiveByCourseId(enrollment.CourseId);
            if (feeTemplate == null)
            {
                return null;
            }
            return new FeeAssessmentResultDto
            {
                Success = true,
                Message = "Fee assessment retrieved successfully.",

                FeeAssessmentId = feeAssessment.FeeAssessmentId,
                AssessedAmount = feeAssessment.Amount,
                CalculationType = feeTemplate?.CalculationType.ToString() ?? "Unknown",
                AssessmentDate = feeAssessment.AssessmentDate,
                DueDate = feeAssessment.DueDate,

                InvoiceId = invoice?.InvoiceId,
                InvoiceNumber = invoice?.InvoiceNumber,
                TotalAmount = invoice?.TotalAmount,
                BalanceDue = invoice?.BalanceDue,

                EnrollmentId = enrollmentId,
                StudentId = enrollment?.StudentId,
                CourseId = enrollment?.CourseId
            };
        }

        #endregion
    }
}
