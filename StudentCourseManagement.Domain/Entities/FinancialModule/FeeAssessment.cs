namespace StudentCourseManagement.Domain.Entities.FinancialModule
{
    public class FeeAssessment
    {
        public int FeeAssessmentId { get; set; }
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public int InvoiceId { get; set; }
        public int FeeTemplateId { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public AssessmentStatus Status { get; set; }
        public bool IsActive { get; set; }

        // 🔄 NEW PROPERTIES ADDED
        public DateTimeOffset? PaidDate { get; set; }      // When fully paid
        public decimal? LateFeeAmount { get; set; }        // Late fee amount (10%)
        public DateTimeOffset? LateFeeAppliedDate { get; set; } // When late fee was add
    }
}
