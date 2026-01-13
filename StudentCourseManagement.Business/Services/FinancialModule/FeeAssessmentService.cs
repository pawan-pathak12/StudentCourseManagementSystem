using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Services.FinancialModule
{
    public class FeeAssessmentService : IFeeAssessmentService
    {
        private readonly IFeeAssessmentRepository _feeAssessmentRepository;
        private readonly ILogger<FeeAssessmentService> _logger;
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IFeeTemplateRepository _feeTemplateRepository;


        public FeeAssessmentService(IFeeAssessmentRepository feeAssessmentRepository, ILogger<FeeAssessmentService> logger, ICourseRepository courseRepository,
            IEnrollmentRepository enrollmentRepository, IFeeTemplateRepository feeTemplateRepository)
        {
            this._feeAssessmentRepository = feeAssessmentRepository;
            this._logger = logger;
            this._courseRepository = courseRepository;
            this._enrollmentRepository = enrollmentRepository;
            this._feeTemplateRepository = feeTemplateRepository;
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
    }
}
