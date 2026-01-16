namespace StudentCourseManagement.Application.DTOs.DTOs.FInancialModule.PaymentMethods
{
    public class UpdatePaymentMethodDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
