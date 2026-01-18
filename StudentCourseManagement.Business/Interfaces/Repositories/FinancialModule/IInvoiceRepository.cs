using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule
{
    public interface IInvoiceRepository
    {
        #region CURD Operations 
        Task<int> AddAsync(Invoice invoice);
        Task<IEnumerable<Invoice>> GetAllAsync();
        Task<Invoice?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, Invoice invoice);
        Task<bool> DeleteAsync(int id);

        #endregion

        #region Phase -3 required method
        Task<string> GenerateInvoiceNumberAsync();
        Task<Invoice?> GetByFeeAssessmentIdAsync(int feeAssessmentId);

        #endregion

        #region Phase 4 : Payment Processing required methods
        Task<bool> UpdatePaymentInfoAsync(int invoiceId, decimal paidAmount, decimal balanceDue, InvoiceStatus status);
        #endregion
    }
}
