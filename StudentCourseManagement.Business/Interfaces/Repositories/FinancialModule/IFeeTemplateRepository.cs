using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule
{
    public interface IFeeTemplateRepository
    {

        #region CURD Operations 
        Task<int> AddAsync(FeeTemplate feeTemplate);
        Task<IEnumerable<FeeTemplate>> GetAllAsync();
        Task<FeeTemplate?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, FeeTemplate feeTemplate);
        Task<bool> DeleteAsync(int id);

        #endregion

        #region Phase-3
        Task<FeeTemplate?> GetActiveByCourseId(int courseId);
        #endregion

    }
}
