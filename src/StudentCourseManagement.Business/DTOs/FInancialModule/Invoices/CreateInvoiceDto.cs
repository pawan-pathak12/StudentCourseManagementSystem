namespace StudentCourseManagement.Application.DTOs.FInancialModule.Invoices
{
    public class CreateInvoiceDto
    {
        public int InvoiceId { get; set; }
        public int CourseId { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public int feeTemplateId { get; set; }
    }
}
