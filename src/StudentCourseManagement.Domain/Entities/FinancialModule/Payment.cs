using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Domain.Entities.FinancialModule
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int StudentId { get; set; }
        public int InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }
        public int PaymentMethodId { get; set; }
        public bool IsActive { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset PaymentDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string ProcessedBy { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }

        //refund 
        public int? RefundedPaymentId { get; set; }
        public string? RefundReason { get; set; }
        public DateTimeOffset RefundDate { get; set; }
    }
}
