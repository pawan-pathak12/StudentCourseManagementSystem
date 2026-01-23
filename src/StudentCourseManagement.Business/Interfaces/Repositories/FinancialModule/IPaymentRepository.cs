using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule
{
    public interface IPaymentRepository
    {
        #region CURD Operations 
        Task<int> AddAsync(Payment payment);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, Payment payment);
        Task<bool> DeleteAsync(int id);

        #endregion

        #region Required Method for Phase 4 : payment processing 
        Task<Payment?> GetByInvoiceIdAsync(int invoiceId);

        #endregion
        #region Phase 5 : refud payment 
        Task<Invoice?> GetInvoiceByPaymentIdAsync(int paymentId);
        Task<bool> IsRefundedAsync(int paymentId);
        Task<int> GetEnrollmentIdFromPaymentIdAsync(int paymentId);
        Task<Payment?> GetRefundPaymentDataByPaymentId(int paymentId);
        #endregion

    }
}
