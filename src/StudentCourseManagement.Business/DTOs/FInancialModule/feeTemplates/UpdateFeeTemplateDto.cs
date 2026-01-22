namespace StudentCourseManagement.Application.DTOs.FInancialModule.feeTemplates
{
    public class UpdateFeeTemplateDto
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
