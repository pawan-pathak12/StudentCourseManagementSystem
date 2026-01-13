using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Interfaces.Services.FinancialModule
{
    public interface IPaymentMethodService
    {
        #region CURD Operations 
        Task<bool> CreateAsync(PaymentMethod paymentMethod);
        Task<IEnumerable<PaymentMethod>> GetAllAsync();
        Task<PaymentMethod?> GetByIdAsync(int paymentMethodId);
        Task<bool> UpdateAsync(int paymentMethodId, PaymentMethod paymentMethod);
        Task<bool> DeleteAsync(int paymentMethodId);

        #endregion


    }
}
