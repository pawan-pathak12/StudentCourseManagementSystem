using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Tests.Unit.TestUtils.Builders.FinancialModule
{
    public class PaymentBuilder
    {
        private int _paymentId;
        private int _studentId;
        private int _invoiceId;
        private int _paymentMethodId;
        private bool _isActive = true;
        private decimal _amount = 0.0m;
        private DateTimeOffset _paymentDate = DateTimeOffset.UtcNow;
        private PaymentStatus _paymentStatus = PaymentStatus.Completed;
        private string _referenceNumber = string.Empty;
        private string? _notes;
        private string _processedBy = string.Empty;
        private DateTimeOffset _createdDate = DateTimeOffset.UtcNow;

        // refund
        private int? _refundedPaymentId;
        private string? _refundReason;
        private DateTimeOffset _refundDate;

        #region WithXX.. Method
        public PaymentBuilder WithPaymentId(int paymentId)
        {
            _paymentId = paymentId;
            return this;
        }

        public PaymentBuilder WithStudentId(int studentId)
        {
            _studentId = studentId;
            return this;
        }

        public PaymentBuilder WithInvoiceId(int invoiceId)
        {
            _invoiceId = invoiceId;
            return this;
        }



        public PaymentBuilder WithPaymentMethodId(int paymentMethodId)
        {
            _paymentMethodId = paymentMethodId;
            return this;
        }

        public PaymentBuilder WithIsActive(bool isActive)
        {
            _isActive = isActive;
            return this;
        }

        public PaymentBuilder WithAmount(decimal amount)
        {
            _amount = amount;
            return this;
        }

        public PaymentBuilder WithPaymentDate(DateTimeOffset paymentDate)
        {
            _paymentDate = paymentDate;
            return this;
        }

        public PaymentBuilder WithPaymentStatus(PaymentStatus status)
        {
            _paymentStatus = status;
            return this;
        }

        public PaymentBuilder WithReferenceNumber(string referenceNumber)
        {
            _referenceNumber = referenceNumber;
            return this;
        }

        public PaymentBuilder WithNotes(string? notes)
        {
            _notes = notes;
            return this;
        }

        public PaymentBuilder WithProcessedBy(string processedBy)
        {
            _processedBy = processedBy;
            return this;
        }

        public PaymentBuilder WithCreatedDate(DateTimeOffset createdDate)
        {
            _createdDate = createdDate;
            return this;
        }

        public PaymentBuilder WithRefundedPaymentId(int? refundedPaymentId)
        {
            _refundedPaymentId = refundedPaymentId;
            return this;
        }

        public PaymentBuilder WithRefundReason(string? refundReason)
        {
            _refundReason = refundReason;
            return this;
        }

        public PaymentBuilder WithRefundDate(DateTimeOffset refundDate)
        {
            _refundDate = refundDate;
            return this;
        }

        #endregion
        public Payment Build()
        {
            return new Payment
            {
                PaymentId = _paymentId,
                StudentId = _studentId,
                InvoiceId = _invoiceId,
                PaymentMethodId = _paymentMethodId,
                IsActive = _isActive,
                Amount = _amount,
                PaymentDate = _paymentDate,
                PaymentStatus = _paymentStatus,
                ReferenceNumber = _referenceNumber,
                Notes = _notes,
                ProcessedBy = _processedBy,
                CreatedDate = _createdDate,
                RefundedPaymentId = _refundedPaymentId,
                RefundReason = _refundReason,
                RefundDate = _refundDate
            };
        }
    }
}
