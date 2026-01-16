namespace StudentCourseManagement.Application.DTOs.DTOs.Students
{
    public class StudentResponseDto
    {
        public int StudentId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTimeOffset EnrollmentDate { get; set; }
        public string? Address { get; set; }
        public DateTimeOffset DOB { get; set; }
        public long Number { get; set; }
        public string? Gender { get; set; }
    }
}
