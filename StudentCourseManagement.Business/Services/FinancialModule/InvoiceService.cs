using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Services.FinancialModule
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILogger<InvoiceService> _logger;
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IFeeAssessmentRepository _feeAssessmentRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository, ILogger<InvoiceService> logger, IStudentRepository studentRepository, ICourseRepository courseRepository, IFeeAssessmentRepository feeAssessmentRepository)
        {
            this._invoiceRepository = invoiceRepository;
            this._logger = logger;
            this._studentRepository = studentRepository;
            this._courseRepository = courseRepository;
            this._feeAssessmentRepository = feeAssessmentRepository;
        }

        #region CURD Operation
        public async Task<bool> CreateAsync(Invoice invoice)
        {
            var student = await _courseRepository.GetByIdAsync(invoice.StudentId);
            if (student == null)
            {
                _logger.LogWarning($"Student with Id {invoice.StudentId} not found");
                return false;
            }
            var course = await _courseRepository.GetByIdAsync(invoice.CourseId);
            if (course == null)
            {
                _logger.LogWarning($"Course with Id {invoice.CourseId} not found");
                return false;
            }
            var feeAssessment = await _feeAssessmentRepository.GetByIdAsync(invoice.FeeAssessmentId);
            if (feeAssessment == null)
            {
                _logger.LogWarning($"FeeAssessment with Id {invoice.FeeAssessmentId} not found");
                return false;
            }

            await _invoiceRepository.AddAsync(invoice);
            return true;
        }

        public async Task<bool> DeleteAsync(int invoiceId)
        {
            var invoice = await GetByIdAsync(invoiceId);
            if (invoice == null)
            {
                _logger.LogWarning($"Invoice with Id {invoiceId} not found");
                return false;
            }
            return await _invoiceRepository.DeleteAsync(invoiceId);
        }

        public async Task<IEnumerable<Invoice>> GetAllAsync()
        {
            return await _invoiceRepository.GetAllAsync();
        }

        public async Task<Invoice?> GetByIdAsync(int invoiceId)
        {
            return await _invoiceRepository.GetByIdAsync(invoiceId);
        }

        public async Task<bool> UpdateAsync(int invoiceId, Invoice invoice)
        {
            if (invoiceId != invoice.InvoiceId)
            {
                _logger.LogWarning("Id mismatched");
                return false;
            }
            var existingInvoice = await _invoiceRepository.GetByIdAsync(invoiceId);
            if (existingInvoice == null)
            {
                _logger.LogWarning($"Update failed : Invoice not found");
                return false;
            }
            return await _invoiceRepository.UpdateAsync(invoiceId, invoice);
        }
        #endregion

    }
}
