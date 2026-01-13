using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Services.FinancialModule
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILogger<InvoiceService> _logger;

        public InvoiceService(IInvoiceRepository invoiceRepository, ILogger<InvoiceService> logger)
        {
            this._invoiceRepository = invoiceRepository;
            this._logger = logger;
        }

        #region CURD Operation
        public async Task<bool> CreateAsync(Invoice invoice)
        {
            await _invoiceRepository.AddAsync(invoice);
            return true;
        }

        public async Task<bool> DeleteAsync(int invoiceId)
        {
            var invoice = await GetByIdAsync(invoiceId);
            if (invoice == null)
            {
                _logger.LogWarning($"Invoice with Id {invoiceId} not found");
                return false;
            }
            return await _invoiceRepository.DeleteAsync(invoiceId);
        }

        public async Task<IEnumerable<Invoice>> GetAllAsync()
        {
            return await _invoiceRepository.GetAllAsync();
        }

        public async Task<Invoice?> GetByIdAsync(int invoiceId)
        {
            return await _invoiceRepository.GetByIdAsync(invoiceId);
        }

        public async Task<bool> UpdateAsync(int invoiceId, Invoice invoice)
        {
            if (invoiceId != invoice.InvoiceId)
            {
                _logger.LogWarning("Id mismatched");
                return false;
            }
            return await _invoiceRepository.UpdateAsync(invoiceId, invoice);
        }
        #endregion

    }
}
