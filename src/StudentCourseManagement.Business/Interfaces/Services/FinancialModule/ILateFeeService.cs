using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Interfaces.Services.FinancialModule
{
    public interface ILateFeeService
    {
        Task<bool> ApplyLateFeeAsync(int invoiceId);
        Task<(int success, int failed)> ProcessAllOverDueAsync();
        Task<Invoice> GetOverDueInvoiceAsync(int invoiceId);
    }
}
