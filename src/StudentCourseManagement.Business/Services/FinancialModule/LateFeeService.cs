using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Constants;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using System.Transactions;

namespace StudentCourseManagement.Business.Services.FinancialModule
{
    public class LateFeeService : ILateFeeService
    {
        private readonly ILogger<LateFeeService> _logger;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IFeeAssessmentRepository _feeAssessmentRepository;
        private readonly IInvoiceLineItemRepository _lineItemRepository;

        public LateFeeService(IInvoiceRepository invoiceRepository, ILogger<LateFeeService> logger
            , IFeeAssessmentRepository feeAssessmentRepository, IInvoiceLineItemRepository lineItemRepository)
        {
            this._invoiceRepository = invoiceRepository;
            this._feeAssessmentRepository = feeAssessmentRepository;
            this._lineItemRepository = lineItemRepository;
            this._logger = logger;
        }

        public async Task<Invoice> GetOverDueInvoiceAsync(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetOverDueInvoiceAsync(invoiceId);
            if (invoice == null)
            {
                _logger.LogWarning("Invoice {InvoiceId} not found.", invoiceId);
                return null;
            }
            return invoice;

        }

        public async Task<bool> ApplyLateFeeAsync(int invoiceId)
        {
            // 1. Get invoice
            var invoice = await GetOverDueInvoiceAsync(invoiceId);
            if (invoice == null)
            {
                _logger.LogWarning("Invoice {InvoiceId} not found.", invoiceId);
                return false;
            }

            if (invoice.InvoiceStatus != InvoiceStatus.Issued &&
                invoice.InvoiceStatus != InvoiceStatus.PartiallyPaid)
            {
                _logger.LogWarning($"Invoice must be payable but current status is {invoice.InvoiceStatus}");
                return false;
            }
            if (invoice.LateFeeApplied)
            {
                _logger.LogWarning("Late Fee is already applied");
                return false;
            }

            //calculate late fee 
            decimal lateFee = invoice.BalanceDue * FinancialConstants.LATE_FEE_PERCENTAGE;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            //update Invoice 
            invoice.TotalAmount = invoice.TotalAmount + lateFee;
            invoice.BalanceDue = invoice.TotalAmount - invoice.AmountPaid;
            invoice.LateFeeApplied = true;
            invoice.InvoiceStatus = InvoiceStatus.Overdue;

            await _invoiceRepository.UpdateAsync(invoice.InvoiceId, invoice);

            //updat feeAssessment 
            var feeAssessment = await _feeAssessmentRepository.GetByIdAsync(invoice.FeeAssessmentId);
            if (feeAssessment != null)
            {
                feeAssessment.LateFeeAmount = lateFee;
                feeAssessment.LateFeeAppliedDate = DateTimeOffset.UtcNow;
                await _feeAssessmentRepository.UpdateAsync(feeAssessment.FeeAssessmentId, feeAssessment);
            }

            // create invocie line item

            var lineItem = new InvoiceLineItem
            {
                InvoiceId = invoiceId,
                CourseId = invoice.CourseId,
                CreatedAt = DateTimeOffset.UtcNow,
                Amount = lateFee,
                Description = "Late Fee",
                IsActive = true,
            };
            await _lineItemRepository.AddAsync(lineItem);
            scope.Complete();
            return true;
            throw new NotImplementedException();
        }

        public async Task<(int success, int failed)> ProcessAllOverDueAsync()
        {
            var invoices = await _invoiceRepository.GetAllOverDueInvoicesAsync();
            int success = 0, failed = 0;

            foreach (var invoice in invoices)
            {
                var result = await ApplyLateFeeAsync(invoice.InvoiceId);
                // FIX: Correct counter logic
                if (result)
                {
                    success++;
                }
                else
                {
                    failed++;
                }
            }

            _logger.LogInformation("Late fee processing completed. Success: {Success}, Failed: {Failed}",
                success, failed);

            return (success, failed);
        }
    }
}
