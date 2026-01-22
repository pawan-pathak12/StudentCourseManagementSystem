using System.ComponentModel.DataAnnotations;

namespace StudentCourseManagement.Application.DTOs.FInancialModule.InvoiceLineItems
{
    public class CreateInvoiceLineItemDto
    {
        [Required]
        public int InvoiceId { get; set; }
        public int CourseId { get; set; }
        [Required]
        public int FeeTemplateId { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
