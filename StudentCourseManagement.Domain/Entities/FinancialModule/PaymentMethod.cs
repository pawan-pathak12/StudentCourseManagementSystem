using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Domain.Entities.FinancialModule
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public string? Name { get; set; }
        public PaymentMethodType PaymentMethodType { get; set; }
        public bool IsActive { get; set; }

    }
}
