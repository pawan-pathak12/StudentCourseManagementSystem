using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Interfaces.Services.FinancialModule
{
    public interface IInvoiceLineItemService
    {
        #region CURD Operations 
        Task<bool> CreateAsync(InvoiceLineItem invoiceLineItem);
        Task<IEnumerable<InvoiceLineItem>> GetAllAsync();
        Task<InvoiceLineItem?> GetByIdAsync(int InvoiceLineItemId);
        Task<bool> UpdateAsync(int InvoiceLineItemID, InvoiceLineItem invoiceLineItem);
        Task<bool> DeleteAsync(int InvoiceLineItemId);

        #endregion


    }
}
