using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Domain.Entities
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public DateTimeOffset EnrollmentDate { get; set; } = DateTimeOffset.UtcNow;
        public int StudentId { get; set; }                  // FK to Student
        public int CourseId { get; set; }                   // FK to Course
        public EnrollmentStatus EnrollmentStatus { get; set; } = EnrollmentStatus.Comfirmed;              // e.g., "Pending", "Confirmed", "Cancelled"                                                                                              //      public bool IsFeePaid { get; set; }                 // Fee status
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreatedAt => DateTime.UtcNow;

        public DateTimeOffset? FeeAssessmentDate { get; set; }     // When fee was calculated
        public DateTimeOffset? CancelledDate { get; set; }       // For refund tracking
        public string? CancellationReason { get; set; }    // Optional: why cancelled
    }
}
