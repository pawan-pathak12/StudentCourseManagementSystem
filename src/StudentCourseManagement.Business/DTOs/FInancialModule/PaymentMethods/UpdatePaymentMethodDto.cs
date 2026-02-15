namespace StudentCourseManagement.Application.DTOs.FInancialModule.PaymentMethods
{
    public class UpdatePaymentMethodDto
    {
        public int PaymentMethodId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
