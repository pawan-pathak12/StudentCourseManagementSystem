using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Application.DTOs.FInancialModule.FeeAssessments
{
    public class UpdateFeeAssessmentDto
    {
        public decimal Amount { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public AssessmentStatus FeeAssessmentStatus { get; set; }
        public DateTimeOffset PaidDate { get; set; }
        public decimal LateFeeAmount { get; set; }
        public DateTimeOffset LateFeeAppliedDate { get; set; }


    }
}
