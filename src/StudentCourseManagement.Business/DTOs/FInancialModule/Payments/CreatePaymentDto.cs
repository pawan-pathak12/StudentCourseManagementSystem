using System.ComponentModel.DataAnnotations;

namespace StudentCourseManagement.Application.DTOs.FInancialModule.Payments
{
    public class CreatePaymentDto
    {

        public int StudentId { get; set; }
        [Required]
        public int InvoiceId { get; set; }
        [Required]
        public int PaymentMethodId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTimeOffset PaymentDate { get; set; }
    }
}
