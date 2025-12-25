using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Business.DTOs.Enrollments
{
    public class UpdateEnrollmentDto
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }          // FK to Student
        public int CourseId { get; set; }           // FK to Course
        public EnrollmentStatus EnrollmentStatus { get; set; } = EnrollmentStatus.Confirmed;
        public DateTimeOffset EnrolledOn { get; set; } = DateTimeOffset.UtcNow;
        public bool IsActive { get; set; } = true;

        public DateTimeOffset? FeeAssessedDate { get; set; }
        public DateTimeOffset? CancelledDate { get; set; }
        public string? CancellationReason { get; set; }
    }
}
