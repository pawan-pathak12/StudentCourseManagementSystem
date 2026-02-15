namespace StudentCourseManagement.Application.DTOs.FInancialModule.feeTemplates
{
    public class UpdateFeeTemplateDto
    {
        public int FeeTemplateId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
