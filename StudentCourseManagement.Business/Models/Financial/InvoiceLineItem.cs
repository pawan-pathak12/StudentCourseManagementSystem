namespace Student_Course_Management_API.Models
{
    public class InvoiceLineItem
    {
        public int InvoiceLineItemId { get; set; }
        public int InvoiceId { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset CreatedAt => DateTimeOffset.UtcNow;
        public int FeeTemplateId { get; set; }
        public int CourseId { get; set; }
    }
}
