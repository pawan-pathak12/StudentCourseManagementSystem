using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Interfaces.Services.FinancialModule
{
    public interface IInvoiceService
    {
        #region CURD Operations 
        Task<(bool success, string? errorMessage, int id)> CreateAsync(Invoice invoice);
        Task<IEnumerable<Invoice>> GetAllAsync();
        Task<Invoice?> GetByIdAsync(int feeAssessmentId);
        Task<bool> UpdateAsync(int invoiceId, Invoice invoice);
        Task<bool> DeleteAsync(int invoiceId);

        #endregion


    }
}
