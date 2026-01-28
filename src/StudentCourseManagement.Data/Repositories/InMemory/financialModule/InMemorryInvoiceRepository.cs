using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Data.Repositories.InMemory.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Data.Repositories.InMemory.financialModule
{
    public class InMemorryInvoiceRepository : IInvoiceRepository
    {
        private readonly List<Invoice> _invoice;
        private readonly IMapper _mapper;
        private readonly InMemoryFeeAssessmentRepository _feeAssessmentRepository;

        public InMemorryInvoiceRepository(IMapper mapper, InMemoryFeeAssessmentRepository feeAssessmentRepository)
        {
            _invoice = new List<Invoice>();
            this._mapper = mapper;
            this._feeAssessmentRepository = feeAssessmentRepository;
        }

        #region CURD Operations 
        public Task<int> AddAsync(Invoice invoice)
        {
            _invoice.Add(invoice);
            invoice.InvoiceId++;
            return Task.FromResult(invoice.InvoiceId);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var invoice = await GetByIdAsync(id);
            if (invoice == null)
            {
                return false;
            }
            invoice.IsActive = false;
            return true;
        }

        public Task<IEnumerable<Invoice>> GetAllAsync()
        {
            var invoices = _invoice.AsEnumerable();
            return Task.FromResult(invoices);
        }

        public Task<Invoice?> GetByIdAsync(int id)
        {
            var invoice = _invoice.Find(x => x.InvoiceId == id && x.IsActive == true);
            return Task.FromResult(invoice);
        }

        public Task<bool> UpdateAsync(int id, Invoice invoice)
        {
            var existingInvoice = _invoice.FirstOrDefault(x => x.InvoiceId == id && x.IsActive == true);
            if (invoice == null)
            {
                return Task.FromResult(false);
            }
            _mapper.Map(existingInvoice, invoice);
            return Task.FromResult(true);
        }
        #endregion

        #region Phase -3 required method
        public Task<string> GenerateInvoiceNumberAsync()
        {
            string invoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
            return Task.FromResult(invoiceNumber);

        }
        public Task<Invoice?> GetByFeeAssessmentIdAsync(int feeAssessmentId)
        {
            var invoice = _invoice.Find(x => x.FeeAssessmentId == feeAssessmentId);
            return Task.FromResult(invoice);

        }


        #endregion

        #region Phase 5 
        public async Task<FeeAssessment?> GetFeeAssessmentByInvoiceIdAsync(int invoiceId)
        {
            var invoice = _invoice.Find(x => x.InvoiceId == invoiceId);
            if (invoice == null)
            {
                return null;
            }
            return await _feeAssessmentRepository.GetByIdAsync(invoice.FeeAssessmentId);

        }

        #endregion

        #region Phase 6
        public Task<Invoice> GetOverDueInvoiceAsync(int invoiceId)
        {
            var invoice = _invoice.Find(x => x.InvoiceId == invoiceId && x.DueDate < DateTimeOffset.UtcNow && x.IsActive == true);
            return Task.FromResult(invoice);
        }

        public Task<IEnumerable<Invoice>> GetAllOverDueInvoicesAsync()
        {
            var invoices = _invoice.Where(x => x.DueDate < DateTimeOffset.UtcNow);
            return Task.FromResult(invoices);
        }
        #endregion



    }
}
