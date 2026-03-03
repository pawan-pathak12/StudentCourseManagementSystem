using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Application.DTOs.FInancialModule.Invoices
{
    public class UpdateInvoiceDto
    {
        public int InvoiceId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int FeeAssessmentId { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }
        public DateTimeOffset PaidDate { get; set; }
        public bool IsActive { get; set; }
    }
}
