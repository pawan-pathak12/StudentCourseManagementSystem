using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Interfaces.Services.FinancialModule
{
    public interface IFeeTemplateService
    {
        #region CURD Operations 
        Task<(bool success, string? errorMessage, int id)> CreateAsync(FeeTemplate feeTemplate);
        Task<IEnumerable<FeeTemplate>> GetAllAsync();
        Task<FeeTemplate?> GetByIdAsync(int feeTemplateId);
        Task<bool> UpdateAsync(int feetemplateId, FeeTemplate feeTemplate);
        Task<bool> DeleteAsync(int feeTemplateId);

        #endregion


    }
}
