using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Domain.Entities.FinancialModule
{
    public class FeeTemplate
    {
        public int FeeTemplateId { get; set; }
        public int CourseId { get; set; }
        public string? Name { get; set; }
        public CalculationType CalculationType { get; set; }
        public decimal Amount { get; set; }
        public decimal RatePerCredit { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }


    }
}
