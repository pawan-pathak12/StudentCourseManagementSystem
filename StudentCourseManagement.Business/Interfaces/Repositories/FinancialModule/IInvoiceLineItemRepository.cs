using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule
{
    public interface IInvoiceLineItemRepository
    {
        #region CURD Operations 
        Task<int> AddAsync(InvoiceLineItem invoicelineItem);
        Task<IEnumerable<InvoiceLineItem>> GetAllAsync();
        Task<InvoiceLineItem> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, InvoiceLineItem invoicelineItem);
        Task<bool> DeleteAsync(int id);

        #endregion
    }
}
