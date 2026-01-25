using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Tests.Unit.TestUtils.Builders.FinancialModule
{
    public class InvoiceBuilder
    {
        private int _invoiceId;
        private string _invoiceNumber = string.Empty;
        private int _studentId;
        private int _courseId;
        private int _feeAssessmentId;
        private bool _isActive = true;
        private decimal _amountPaid;
        private decimal _balanceDue;
        private decimal _totalAmount;
        private DateTimeOffset? _dueDate;
        private InvoiceStatus _invoiceStatus;
        private DateTimeOffset _createdAt = DateTimeOffset.UtcNow;
        private bool _lateFeeApplied = false;
        private DateTimeOffset _issuedAt;
        private DateTimeOffset _updatedAt;
        private decimal _discount = 0;

        public InvoiceBuilder WithInvoiceId(int invoiceId)
        {
            _invoiceId = invoiceId;
            return this;
        }

        public InvoiceBuilder WithInvoiceNumber(string invoiceNumber)
        {
            _invoiceNumber = invoiceNumber;
            return this;
        }

        public InvoiceBuilder WithStudentId(int studentId)
        {
            _studentId = studentId;
            return this;
        }

        public InvoiceBuilder WithCourseId(int courseId)
        {
            _courseId = courseId;
            return this;
        }

        public InvoiceBuilder WithFeeAssessmentId(int feeAssessmentId)
        {
            _feeAssessmentId = feeAssessmentId;
            return this;
        }

        public InvoiceBuilder WithIsActive(bool isActive)
        {
            _isActive = isActive;
            return this;
        }

        public InvoiceBuilder WithAmountPaid(decimal amountPaid)
        {
            _amountPaid = amountPaid;
            return this;
        }

        public InvoiceBuilder WithBalanceDue(decimal balanceDue)
        {
            _balanceDue = balanceDue;
            return this;
        }

        public InvoiceBuilder WithTotalAmount(decimal totalAmount)
        {
            _totalAmount = totalAmount;
            return this;
        }

        public InvoiceBuilder WithDueDate(DateTimeOffset? dueDate)
        {
            _dueDate = dueDate;
            return this;
        }

        public InvoiceBuilder WithInvoiceStatus(InvoiceStatus status)
        {
            _invoiceStatus = status;
            return this;
        }

        public InvoiceBuilder WithCreatedAt(DateTimeOffset createdAt)
        {
            _createdAt = createdAt;
            return this;
        }

        public InvoiceBuilder WithLateFeeApplied(bool lateFeeApplied)
        {
            _lateFeeApplied = lateFeeApplied;
            return this;
        }

        public InvoiceBuilder WithIssuedAt(DateTimeOffset issuedAt)
        {
            _issuedAt = issuedAt;
            return this;
        }

        public InvoiceBuilder WithUpdatedAt(DateTimeOffset updatedAt)
        {
            _updatedAt = updatedAt;
            return this;
        }

        public InvoiceBuilder WithDiscount(decimal discount)
        {
            _discount = discount;
            return this;
        }

        public Invoice Build()
        {
            return new Invoice
            {
                InvoiceId = _invoiceId,
                InvoiceNumber = _invoiceNumber,
                StudentId = _studentId,
                CourseId = _courseId,
                FeeAssessmentId = _feeAssessmentId,
                IsActive = _isActive,
                AmountPaid = _amountPaid,
                BalanceDue = _balanceDue,
                TotalAmount = _totalAmount,
                DueDate = _dueDate,
                InvoiceStatus = _invoiceStatus,
                CreatedAt = _createdAt,
                LateFeeApplied = _lateFeeApplied,
                IssuedAt = _issuedAt,
                UpdatedAt = _updatedAt,
                Discount = _discount
            };
        }
    }
}
