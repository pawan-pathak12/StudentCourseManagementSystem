namespace StudentCourseManagement.Application.DTOs.DTOs.FInancialModule.Invoices
{
    public class CreateInvoiceDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public int FeeAssessmentId { get; set; }
    }
}
