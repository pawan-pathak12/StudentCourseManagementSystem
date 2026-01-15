using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.API.DTOs.FInancialModule.Invoices
{
    public class UpdateInvoiceDto
    {
        public InvoiceStatus InvoiceStatus { get; set; }
        public DateTimeOffset PaidDate { get; set; }
        public bool IsActive { get; set; }
    }
}
