namespace StudentCourseManagement.Application.DTOs.FInancialModule.feeTemplates
{
    public class CreateFeeTemplateDto
    {
        public string? Name { get; set; }
        public int CourseId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }

    }
}
