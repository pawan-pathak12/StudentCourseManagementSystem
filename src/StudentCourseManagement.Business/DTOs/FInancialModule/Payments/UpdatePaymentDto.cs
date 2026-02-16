using System.ComponentModel.DataAnnotations;

namespace StudentCourseManagement.Application.DTOs.FInancialModule.Payments
{
    public class UpdatePaymentDto
    {
        public int PaymentId { get; set; }
        [Required]
        public int PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset PaymentDate { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
