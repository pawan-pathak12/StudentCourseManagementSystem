using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.DTOs.FInancialModule.Refunds;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Constants;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using System.Transactions;

namespace StudentCourseManagement.Business.Services.FinancialModule
{
    public class RefundService : IRefundService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IFeeAssessmentRepository _feeAssessmentRepository;
        private readonly ILogger<RefundService> _logger;

        public RefundService(IPaymentRepository paymentRepository, ICourseRepository courseRepository, IInvoiceRepository invoiceRepository, IFeeAssessmentRepository feeAssessmentRepository, ILogger<RefundService> logger)
        {
            this._paymentRepository = paymentRepository;
            this._courseRepository = courseRepository;
            this._invoiceRepository = invoiceRepository;
            this._feeAssessmentRepository = feeAssessmentRepository;
            this._logger = logger;
        }
        public async Task<(bool success, string? errorMessage)> ProcessRefundAsync(int paymentId, string? refundReason)
        {
            //1. check if it is refund able eor not 
            var (sucess, errorMessage) = await ValidateEligibilityAsync(paymentId);
            if (!sucess)
            {
                return (false, errorMessage);
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            //2. Create record for refunding 
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                return (false, "Payment not found.");
            }

            var RefundPayment = new Payment
            {
                InvoiceId = payment.InvoiceId,
                PaymentMethodId = payment.PaymentMethodId,
                Amount = -payment.Amount,
                PaymentDate = DateTimeOffset.UtcNow,
                PaymentStatus = PaymentStatus.Refunded,
                RefundReason = refundReason,
                RefundDate = DateTimeOffset.UtcNow,
                RefundedPaymentId = payment.PaymentId,
                IsActive = true,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await _paymentRepository.AddAsync(RefundPayment);

            //3. Update orginal pamyent status 
            payment.PaymentStatus = PaymentStatus.Refunded;
            await _paymentRepository.UpdateAsync(paymentId, payment);

            //4. Update Invoice 
            var invoice = await _paymentRepository.GetInvoiceByPaymentIdAsync(paymentId);
            if (invoice == null)
            {
                return (false, "Invoice not found for this payment.");
            }
            var previousStatus = invoice.InvoiceStatus;
            invoice.AmountPaid -= payment.Amount;
            invoice.BalanceDue = invoice.TotalAmount - invoice.AmountPaid;

            if (invoice.BalanceDue > 0 && invoice.BalanceDue < invoice.TotalAmount)
            {
                invoice.InvoiceStatus = InvoiceStatus.PartiallyPaid;
            }
            if (invoice.BalanceDue == invoice.TotalAmount)
            {
                invoice.InvoiceStatus = InvoiceStatus.Issued;
            }
            await _invoiceRepository.UpdateAsync(invoice.InvoiceId, invoice);

            //5. Update FeeAssessment (if was invoice status paid) 

            if (previousStatus == InvoiceStatus.Paid)
            {
                var feeAssessment = await _invoiceRepository.GetFeeAssessmentByInvoiceIdAsync(invoice.InvoiceId);
                if (feeAssessment != null)
                {
                    feeAssessment.PaidDate = null;
                    feeAssessment.FeeAssessmentStatus = AssessmentStatus.Assessed;
                    await _feeAssessmentRepository.UpdateAsync(feeAssessment.FeeAssessmentId, feeAssessment);
                }
            }
            scope.Complete();
            return (true, null);
        }

        public async Task<(bool sucess, string? errorMessage)> ValidateEligibilityAsync(int paymentId)
        {
            //1. check payment exists or not 
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                _logger.LogWarning($"Refund failed : Payment with Id {paymentId} not found.");
                return (false, $"Payment with Id {paymentId} not found.");
            }

            //2.Payment Status must be completed
            if (payment.PaymentStatus != PaymentStatus.Completed)
            {
                _logger.LogWarning($"Refund failed : Payment status must be completed to refund");
                return (false, $"Refund failed : Payment status must be completed to refund");
            }

            //3.payment must not be refunded already 
            if (payment.PaymentStatus == PaymentStatus.Refunded)
            {
                _logger.LogWarning("Refund Failed: Payment Id {PaymentId} is already refunded.", paymentId);
                return (false, $"Payment Id {paymentId} is already refunded.");
            }

            //4.Rule to be refunded : will be only refunded if it is within 30 days of payment is done 
            if (payment.PaymentDate.AddDays(FinancialConstants.REFUND_WINDOW_DAYS) < DateTimeOffset.UtcNow)
            {
                _logger.LogWarning("Refund Failed: Payment Id {PaymentId} is outside 30-day refund window.", paymentId);
                return (false, "Payment is outside the 30-day refund window.");
            }

            //5 : only refunded till : courseStrtDate+2 days 
            var enrollmentId = await _paymentRepository.GetEnrollmentIdFromPaymentIdAsync(paymentId);
            var courseStartDate = await _courseRepository.GetStartDateByEnrollmentIdAsync(enrollmentId);
            if (courseStartDate.AddDays(FinancialConstants.COURSE_START_BUFFER_DAYS) < DateTimeOffset.UtcNow)
            {
                _logger.LogWarning("Refund Failed: Course started more than 2 days ago for Payment Id {PaymentId}.", paymentId);
                return (false, "Course has started more than 2 days ago. Refund not allowed.");
            }
            return (true, null);
        }

        public async Task<RefundResultDto?> GetRefundInfoAsync(int paymentId)
        {
            var refundData = await _paymentRepository.GetRefundPaymentDataByPaymentId(paymentId);
            if (refundData == null)
            {
                return null;
            }
            return new RefundResultDto
            {
                InvoiceId = refundData.InvoiceId,
                Amount = refundData.Amount,
                IsActive = refundData.IsActive,
                CreatedDate = refundData.CreatedDate,
                PaymentDate = refundData.PaymentDate,
                PaymentMethodId = refundData.PaymentMethodId,
                PaymentStatus = refundData.PaymentStatus,
                RefundDate = refundData.RefundDate,
                RefundedPaymentId = refundData.RefundedPaymentId,
                RefundReason = refundData.RefundReason
            };
        }
    }
}
