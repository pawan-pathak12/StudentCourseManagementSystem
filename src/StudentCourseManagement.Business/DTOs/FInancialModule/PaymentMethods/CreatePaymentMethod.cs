using System.ComponentModel.DataAnnotations;

namespace StudentCourseManagement.Application.DTOs.FInancialModule.PaymentMethods
{
    public class CreatePaymentMethodDto
    {
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
