namespace StudentCourseManagement.Application.DTOs.FInancialModule.feeTemplates
{
    public class UpdateFeeTemplateDto
    {
        public string? Name { get; set; }
        public int CourseId { get; set; }
        public int FeeTemplateId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
