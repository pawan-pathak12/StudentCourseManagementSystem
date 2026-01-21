using System.ComponentModel.DataAnnotations;

namespace StudentCourseManagement.Application.DTOs.FInancialModule.Payments
{
    public class ProcessPaymentDto
    {
        [Required]
        public int InvoiceId { get; set; }
        [Required]
        public int PaymentMethodId { get; set; }
        [Required]
        public decimal PaidAmount { get; set; }
    }
}
