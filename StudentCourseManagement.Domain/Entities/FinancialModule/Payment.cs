namespace StudentCourseManagement.Domain.Entities.FinancialModule
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int StudentId { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset PaymentDate { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentStatus Status { get; set; }
        public string ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public string ProcessedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
