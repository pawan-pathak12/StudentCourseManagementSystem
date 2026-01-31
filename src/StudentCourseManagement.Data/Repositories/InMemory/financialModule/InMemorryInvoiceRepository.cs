using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Data.Repositories.InMemory.financialModule
{
    public class InMemorryInvoiceRepository : IInvoiceRepository
    {
        private readonly List<Invoice> _invoice;
        private readonly List<FeeAssessment> _feeAssessments;
        private readonly IMapper _mapper;
        private readonly InMemoryDbContext _db;

        public InMemorryInvoiceRepository(IMapper mapper, InMemoryDbContext db)
        {
            this._db = db;
            this._feeAssessments = _db.FeeAssessments;
            _invoice = _db.Invoices;
            this._mapper = mapper;
        }

        #region CURD Operations 
        public Task<int> AddAsync(Invoice invoice)
        {
            if (invoice.InvoiceId == 0)
            {
                invoice.InvoiceId = _invoice.Count + 1;
            }

            invoice.IsActive = true;
            _invoice.Add(invoice);
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
            if (existingInvoice == null)
            {
                return Task.FromResult(false);
            }
            _mapper.Map(invoice, existingInvoice);
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
        public Task<FeeAssessment?> GetFeeAssessmentByInvoiceIdAsync(int invoiceId)
        {
            var invoice = _invoice.Find(x => x.InvoiceId == invoiceId);
            if (invoice == null)
            {
                return null;
            }
            var feeAssessment = _feeAssessments.Find(x => x.FeeAssessmentId == invoice.FeeAssessmentId);
            return Task.FromResult(feeAssessment);
        }

        #endregion

        #region Phase 6
        public Task<Invoice?> GetOverDueInvoiceAsync(int invoiceId)
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
