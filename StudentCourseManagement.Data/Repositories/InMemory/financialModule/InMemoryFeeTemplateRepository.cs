using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Data.Repositories.InMemory.financialModule
{
    public class InMemoryFeeTemplateRepository : IFeeTemplateRepository
    {
        private readonly List<FeeTemplate> _feeTemplate;
        private readonly IMapper _mapper;

        public InMemoryFeeTemplateRepository(IMapper mapper)
        {
            _feeTemplate = new List<FeeTemplate>();
            this._mapper = mapper;
        }

        #region CURD Operation
        public Task<int> AddAsync(FeeTemplate feeTemplate)
        {
            _feeTemplate.Add(feeTemplate);
            feeTemplate.FeeTemplateId += 1;
            return Task.FromResult(feeTemplate.FeeTemplateId);

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var feeTemplate = await GetByIdAsync(id);
            if (feeTemplate == null)
            {
                return false;
            }
            feeTemplate.IsActive = false;
            return true;
        }

        public Task<IEnumerable<FeeTemplate>> GetAllAsync()
        {
            var feeTemplates = _feeTemplate.AsEnumerable();
            return Task.FromResult(feeTemplates);
        }

        public Task<FeeTemplate?> GetByIdAsync(int id)
        {
            var feeTemplate = _feeTemplate.Find(x => x.FeeTemplateId == id && x.IsActive == true);
            return Task.FromResult(feeTemplate);
        }

        public async Task<bool> UpdateAsync(int id, FeeTemplate feeTemplate)
        {
            feeTemplate.FeeTemplateId = id;
            var existingFeeTemplate = await GetByIdAsync(id);
            if (existingFeeTemplate == null)
            {
                return false;
            }
            _mapper.Map(existingFeeTemplate, feeTemplate);
            return true;
        }
        #endregion

        #region Phase-3
        public Task<FeeTemplate?> GetActiveByCourseId(int courseId)
        {
            throw new NotImplementedException();

        }
        #endregion
    }
}
