using StudentCourseManagement.Domain.Entities.FinancialModule;

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

        #region Phase 5 : required method 
        Task<FeeAssessment?> GetFeeAssessmentByInvoiceIdAsync(int invoiceId);
        #endregion

        #region Phase 5 

        Task<Invoice> GetOverDueInvoiceAsync(int invoiceId);
        Task<IEnumerable<Invoice?>> GetAllOverDueInvoicesAsync();
        #endregion
    }
}
