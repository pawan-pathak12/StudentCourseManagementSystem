using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Services.FinancialModule
{
    public class FeeTemplateService : IFeeTemplateService
    {
        private readonly IFeeTemplateRepository _feeTemplateRepository;
        private readonly ILogger<FeeTemplateService> _logger;
        private readonly ICourseRepository _courseRepository;

        public FeeTemplateService(IFeeTemplateRepository feeTemplateRepository, ILogger<FeeTemplateService> logger, ICourseRepository courseRepository)
        {
            this._feeTemplateRepository = feeTemplateRepository;
            this._logger = logger;
            this._courseRepository = courseRepository;
        }

        #region CURD Operation
        public async Task<(bool success, string? errorMessage, int id)> CreateAsync(FeeTemplate feeTemplate)
        {
            var course = await _courseRepository.GetByIdAsync(feeTemplate.CourseId);
            if (course == null)
            {
                _logger.LogWarning($"Course with Id {feeTemplate.CourseId} is inactive or not found");
                return (true, $"Course with Id {feeTemplate.CourseId} is inactive or not found", 0);
            }
            var feeTemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);
            return (true, null, feeTemplateId);
        }

        public async Task<bool> DeleteAsync(int feeTemplateId)
        {
            var feeTemplate = await _feeTemplateRepository.GetByIdAsync(feeTemplateId);
            if (feeTemplate == null)
            {
                _logger.LogWarning($"FeeTemplate with Id {feeTemplateId} not found");
                return false;
            }
            return await _feeTemplateRepository.DeleteAsync(feeTemplateId);
        }

        public async Task<IEnumerable<FeeTemplate>> GetAllAsync()
        {
            var feeTemplates = await _feeTemplateRepository.GetAllAsync();
            if (!feeTemplates.Any() || feeTemplates == null)
            {
                return Enumerable.Empty<FeeTemplate>();
            }
            return feeTemplates;
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
            var existingCourse = await _courseRepository.GetByIdAsync(feeTemplate.CourseId);
            if (existingCourse == null)
            {
                _logger.LogWarning($"Course with Id {feeTemplate.CourseId} not found");
                return false;

            }
            var exisitngFeetemplate = await _feeTemplateRepository.GetByIdAsync(feeTemplateId);
            if (exisitngFeetemplate == null)
            {
                _logger.LogWarning($"FeeTemplate with Id {feeTemplateId} not found");
                return false;
            }
            return await _feeTemplateRepository.UpdateAsync(feeTemplateId, feeTemplate);
        }

        #endregion

    }

}
