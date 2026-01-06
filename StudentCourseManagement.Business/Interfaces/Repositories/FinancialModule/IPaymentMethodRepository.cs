using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule
{
    public interface IPaymentMethodRepository
    {
        #region CURD Operations 
        Task<int> AddAsync(PaymentMethod paymentMethod);
        Task<IEnumerable<PaymentMethod>> GetAllAsync();
        Task<PaymentMethod?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, PaymentMethod paymentMethod);
        Task<bool> DeleteAsync(int id);

        #endregion
    }
}
