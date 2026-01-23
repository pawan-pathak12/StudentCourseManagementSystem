using StudentCourseManagement.Business.DTOs.FInancialModule.Refunds;

namespace StudentCourseManagement.Business.Interfaces.Services.FinancialModule
{
    public interface IRefundService
    {
        Task<(bool success, string? errorMessage)> ProcessRefundAsync(int paymentId, string? refundReason);
        Task<(bool sucess, string? errorMessage)> ValidateEligibilityAsync(int paymentId);
        Task<RefundResultDto?> GetRefundInfoAsync(int paymentId);
    }
}
