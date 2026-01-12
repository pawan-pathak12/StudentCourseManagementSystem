using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Services.FinancialModule
{
    public class FeeAssessmentService : IFeeAssessmentService
    {
        private readonly IFeeAssessmentRepository _feeAssessmentRepository;
        private readonly ILogger<FeeAssessmentService> _logger;

        public FeeAssessmentService(IFeeAssessmentRepository feeAssessmentRepository, ILogger<FeeAssessmentService> logger)
        {
            this._feeAssessmentRepository = feeAssessmentRepository;
            this._logger = logger;
        }
        #region CURD Operations 
        public async Task<bool> CreateAsync(FeeAssessment feeAssessment)
        {
            await _feeAssessmentRepository.AddAsync(feeAssessment);
            return true;
        }

        public async Task<IEnumerable<FeeAssessment>> GetAllAsync()
        {
            return await _feeAssessmentRepository.GetAllAsync();
        }

        public async Task<FeeAssessment?> GetByIdAsync(int feeAssessmentId)
        {
            return await _feeAssessmentRepository.GetByIdAsync(feeAssessmentId);
        }

        public async Task<bool> UpdateAsync(int feeAssessmentId, FeeAssessment feeAssessment)
        {
            if (feeAssessmentId != feeAssessment.FeeAssessmentId)
            {
                _logger.LogWarning($"Id mismatched");
                return false;
            }
            return await _feeAssessmentRepository.UpdateAsync(feeAssessmentId, feeAssessment);
        }

        public async Task<bool> DeleteAsync(int feeAssessmentId)
        {
            return await _feeAssessmentRepository.DeleteAsync(feeAssessmentId);
        }

        #endregion
    }
}
