using StudentCourseManagement.Application.DTOs.DTOs.FInancialModule.FeeAssessments;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Interfaces.Services.FinancialModule
{
    public interface IFeeAssessmentService
    {
        #region CURD Operations 
        Task<(bool success, string? errorMessage, int id)> CreateAsync(FeeAssessment feeAssessment);
        Task<IEnumerable<FeeAssessment>> GetAllAsync();
        Task<FeeAssessment?> GetByIdAsync(int feeAssessmentId);
        Task<bool> UpdateAsync(int feeAssessmentId, FeeAssessment feeAssessment);
        Task<bool> DeleteAsync(int feeAssessmentId);

        #endregion
        Task<FeeAssessmentResultDto?> GetFeeAssessmentDetailsByEnrollmentIdAsync(int enrollmentId);
        Task<(bool success, string? ErrorMessage)> AssessFee(int enrollmentId);

    }
}
