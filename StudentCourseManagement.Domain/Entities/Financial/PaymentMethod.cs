using Student_Course_Management_API.Enums;

namespace Student_Course_Management_API.Models
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public PaymentMethodType MethodType { get; set; }
        public string? Provider { get; set; }
        public bool IsActive { get; set; }

    }
}
