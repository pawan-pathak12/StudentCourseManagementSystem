using System.ComponentModel.DataAnnotations;

namespace StudentCourseManagement.API.DTOs.FInancialModule.FeeAssessments
{
    public class CreateFeeAssessmentDto
    {
        [Required]
        public int EnrollmentId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public int FeeAssessmentId { get; set; }
        public decimal Amount { get; set; }
        [Required]
        public DateTimeOffset DueDate { get; set; }
    }
}
