namespace StudentCourseManagement.Domain.Entities
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public DateTime EnrolledOn { get; set; } = DateTime.UtcNow;         // Enrollment date

        public int StudentId { get; set; }                  // FK to Student
        public int CourseId { get; set; }                   // FK to Course
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Confirmed;              // e.g., "Pending", "Confirmed", "Cancelled"                                                                                              //      public bool IsFeePaid { get; set; }                 // Fee status
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAt => DateTime.UtcNow;

        public DateTime? FeeAssessedDate { get; set; }     // When fee was calculated
        public DateTime? CancelledDate { get; set; }       // For refund tracking
        public string? CancellationReason { get; set; }    // Optional: why cancelled
    }
}
