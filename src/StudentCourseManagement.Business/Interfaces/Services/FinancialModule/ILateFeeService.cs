using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Interfaces.Services.FinancialModule
{
    public interface ILateFeeService
    {
        Task<bool> ApplyLateFeeAsync(int invoiceId);
        Task ProcessAllOverDue();
        Task<Invoice> GetOverDueInvoiceAsync(int invoiceId);
    }
}
