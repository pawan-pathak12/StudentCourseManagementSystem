namespace StudentCourseManagement.API.DTOs
{
    public class CourseResponseDto
    {
        public int CourseId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public int Capacity { get; set; }
        public DateTimeOffset EnrollmentStartDate { get; set; }
        public DateTimeOffset EnrollmentEndDate { get; set; }
        public bool IsActive { get; set; }


    }
}
