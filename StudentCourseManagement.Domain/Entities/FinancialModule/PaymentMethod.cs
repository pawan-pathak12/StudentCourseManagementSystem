namespace StudentCourseManagement.Domain.Entities.FinancialModule
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public PaymentMethodType MethodType { get; set; }
        public string? Provider { get; set; }
        public bool IsActive { get; set; }

    }
}
