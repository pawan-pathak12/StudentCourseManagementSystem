using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Domain.Entities.FinancialModule
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public PaymentMethodType PaymentMethodType { get; set; }
        public string? Provider { get; set; }
        public bool IsActive { get; set; }

    }
}
