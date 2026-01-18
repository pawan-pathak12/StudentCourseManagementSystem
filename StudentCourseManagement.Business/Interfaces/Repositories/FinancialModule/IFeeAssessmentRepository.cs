using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule
{
    public interface IFeeAssessmentRepository
    {
        #region CURD Operations 
        Task<int> AddAsync(FeeAssessment feeAssessment);
        Task<IEnumerable<FeeAssessment>> GetAllAsync();
        Task<FeeAssessment?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, FeeAssessment feeAssessment);
        Task<bool> DeleteAsync(int id);

        #endregion

        #region Phase -3 required method 
        Task<bool> ExistsByEnrollmentIdAsync(int enrollmentId);
        Task<FeeAssessment?> GetByEnrolmentIdAsync(int enrollmentId);
        #endregion

        #region Phase -4 : Paymnent Processing Required Methods 
        Task<FeeAssessment?> GetByInvoiceIdAsync(int invoiceId);
        Task<bool> UpdatePaymentStatusAsync(int invoiceId);

        #endregion

    }
}
