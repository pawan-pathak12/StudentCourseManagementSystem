using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Domain.Entities.FinancialModule
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int FeeAssessmentId { get; set; }
        public bool IsActive { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal BalanceDue { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public bool LateFeeApplied { get; set; } = false;
        public DateTimeOffset IssuedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public decimal Discount { get; set; } = 0;
    }
}
