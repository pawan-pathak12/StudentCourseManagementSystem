using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Interfaces.Services.FinancialModule
{
    public interface IFeeAssessmentService
    {
        #region CURD Operations 
        Task<bool> CreateAsync(FeeAssessment feeAssessment);
        Task<IEnumerable<FeeAssessment>> GetAllAsync();
        Task<FeeAssessment?> GetByIdAsync(int feeAssessmentId);
        Task<bool> UpdateAsync(int feeAssessmentId, FeeAssessment feeAssessment);
        Task<bool> DeleteAsync(int feeAssessmentId);

        #endregion


    }
}
