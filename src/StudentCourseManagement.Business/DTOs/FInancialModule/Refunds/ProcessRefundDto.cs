using System.ComponentModel.DataAnnotations;

namespace StudentCourseManagement.Business.DTOs.FInancialModule.Refunds
{
    public class ProcessRefundDto
    {
        [Required]
        public int PaymentId { get; set; }
        [Required]
        public string? RefundReason { get; set; }
    }
}
