using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Application.DTOs.FInancialModule.Payments
{
    public class PaymentResultDto
    {
        public int PaymentId { get; set; }
        public int InvoiceId { get; set; }
        public int FeeAssessmentId { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal TotalAmount { get; set; } // total invoice amount
        public decimal PaidAmount { get; set; }
        public decimal BalanceDue { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }
    }
}
