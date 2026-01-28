using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Data.Repositories.InMemory.financialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Data.Repositories.InMemory.FinancialModule
{
    public class InMemoryFeeAssessmentRepository : IFeeAssessmentRepository
    {
        private readonly List<FeeAssessment> _feeAssessment;
        private readonly List<Invoice> _invoices;
        private readonly IMapper _mapper;
        private readonly InMemorryInvoiceRepository _invoiceRepository;

        public InMemoryFeeAssessmentRepository(IMapper mapper, InMemorryInvoiceRepository invoiceRepository)
        {
            _feeAssessment = new List<FeeAssessment>();
            _invoices = new List<Invoice>();
            _mapper = mapper;
            this._invoiceRepository = invoiceRepository;
        }

        #region CRUD Operation

        public Task<int> AddAsync(FeeAssessment feeAssessment)
        {
            // Optionally generate ID if not set
            if (feeAssessment.FeeAssessmentId == 0)
            {
                feeAssessment.FeeAssessmentId = _feeAssessment.Count + 1;
            }

            _feeAssessment.Add(feeAssessment);
            return Task.FromResult(feeAssessment.FeeAssessmentId);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var feeAssessment = await GetByIdAsync(id);
            if (feeAssessment == null)
            {
                return false;
            }
            feeAssessment.IsActive = false;
            return true;
        }

        public Task<IEnumerable<FeeAssessment>> GetAllAsync()
        {
            return Task.FromResult(_feeAssessment.AsEnumerable());
        }

        public Task<FeeAssessment?> GetByIdAsync(int id)
        {
            var feeAssessment = _feeAssessment.Find(x => x.FeeAssessmentId == id && x.IsActive == true);
            return Task.FromResult(feeAssessment);
        }

        public async Task<bool> UpdateAsync(int id, FeeAssessment feeAssessment)
        {
            var existingFeeAssessment = await GetByIdAsync(id);
            if (existingFeeAssessment == null)
            {
                return false;
            }

            _mapper.Map(feeAssessment, existingFeeAssessment);
            return true;
        }

        #endregion

        #region Phase -3 required method 
        public Task<bool> ExistsByEnrollmentIdAsync(int enrollmentId)
        {
            var feeAssessment = _feeAssessment.Exists(x => x.EnrollmentId == enrollmentId);
            return Task.FromResult(feeAssessment);
        }
        public Task<FeeAssessment?> GetByEnrolmentIdAsync(int enrollmentId)
        {
            var feeAssessment = _feeAssessment.Find(x => x.EnrollmentId == enrollmentId);
            return Task.FromResult(feeAssessment);

        }
        #endregion

        #region Phase -4 : payment Processing required method
        public async Task<FeeAssessment?> GetByInvoiceIdAsync(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
            if (invoice == null)
            {
                return null;
            }
            return _feeAssessment.FirstOrDefault(f => f.FeeAssessmentId == invoice?.FeeAssessmentId && f.IsActive == true);
        }



        #endregion
    }
}