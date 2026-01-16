namespace StudentCourseManagement.API.DTOs.FInancialModule.FeeAssessments
{
    public class FeeAssessmentResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        // FeeAssessment Details
        public int? FeeAssessmentId { get; set; }
        public decimal? AssessedAmount { get; set; }
        public string? CalculationType { get; set; }
        public DateTimeOffset? AssessmentDate { get; set; }
        public DateTimeOffset? DueDate { get; set; }

        // Invoice Details
        public int? InvoiceId { get; set; }
        public string? InvoiceNumber { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? BalanceDue { get; set; }

        // Related Info
        public int? EnrollmentId { get; set; }
        public string? StudentName { get; set; }
        public int? StudentId { get; set; }
        public int? CourseId { get; set; }
    }
}
