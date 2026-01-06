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
    }
}
