using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Interfaces.Services.FinancialModule
{
    public interface IInvoiceLineItemService
    {
        #region CURD Operations 
        Task<(bool success, string? errorMessage, int id)> CreateAsync(InvoiceLineItem invoiceLineItem);
        Task<IEnumerable<InvoiceLineItem>> GetAllAsync();
        Task<InvoiceLineItem?> GetByIdAsync(int InvoiceLineItemId);
        Task<bool> UpdateAsync(int InvoiceLineItemID, InvoiceLineItem invoiceLineItem);
        Task<bool> DeleteAsync(int InvoiceLineItemId);

        #endregion


    }
}
