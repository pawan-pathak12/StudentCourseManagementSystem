using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Business.DTOs.FInancialModule.Refunds
{
    public class RefundResultDto
    {
        public int InvoiceId { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset PaymentDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string RefundReason { get; set; } = string.Empty;
        public DateTimeOffset RefundDate { get; set; }
        public int RefundedPaymentId { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedDate { get; set; }


    }
}
