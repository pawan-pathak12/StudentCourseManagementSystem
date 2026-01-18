using StudentCourseManagement.Application.DTOs.DTOs.FInancialModule.Payments;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Interfaces.Services.FinancialModule
{
    public interface IPaymentService
    {
        #region CURD Operations 
        Task<bool> CreateAsync(Payment payment);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(int PaymentId);
        Task<bool> UpdateAsync(int paymentId, Payment payment);
        Task<bool> DeleteAsync(int PaymentId);

        #endregion

        Task<(bool success, string ErrorMessage)> ProcessPaymentAsync(int invoiceId, int paymentMethodId, decimal paidAmount);
        Task<PaymentResultDto> GetPaymentDetailsByInvoiceIdAsync(int invoiceId);


    }
}
