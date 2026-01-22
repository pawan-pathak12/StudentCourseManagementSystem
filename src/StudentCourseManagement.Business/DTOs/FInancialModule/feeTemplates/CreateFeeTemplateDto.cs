namespace StudentCourseManagement.Application.DTOs.FInancialModule.feeTemplates
{
    public class CreateFeeTemplateDto
    {
        public int CourseId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }

    }
}
