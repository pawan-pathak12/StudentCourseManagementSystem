using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Mapping.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Services.FinancialModule
{
    public class FeeTemplateService : IFeeTemplateService
    {
        private readonly IFeeTemplateRepository _feeTemplateRepository;
        private readonly ILogger<FeeTemplateService> _logger;

        public FeeTemplateService(IFeeTemplateRepository feeTemplateRepository, ILogger<FeeTemplateService> logger)
        {
            this._feeTemplateRepository = feeTemplateRepository;
            this._logger = logger;
        }

        #region CURD Operation
        public async Task<bool> CreateAsync(FeeTemplate feeTemplate)
        {
            await _feeTemplateRepository.AddAsync(feeTemplate);
            return true;
        }

        public async Task<bool> DeleteAsync(int feeTemplateId)
        {
            return await _feeTemplateRepository.DeleteAsync(feeTemplateId);
        }

        public async Task<IEnumerable<FeeTemplate>> GetAllAsync()
        {
            return await _feeTemplateRepository.GetAllAsync();
        }

        public async Task<FeeTemplate?> GetByIdAsync(int feeTemplateId)
        {
            return await _feeTemplateRepository.GetByIdAsync(feeTemplateId);
        }

        public async Task<bool> UpdateAsync(int feeTemplateId, FeeTemplate feeTemplate)
        {
            if (feeTemplateId != feeTemplate.FeeTemplateId)
            {
                _logger.LogWarning("Id MisMatched");
                return false;
            }

            return await _feeTemplateRepository.UpdateAsync(feeTemplateId, feeTemplate);
        }

        #endregion

    }

}
