namespace StudentCourseManagement.Application.DTOs.Courses
{
    public class CreateCourseDto
    {

        public string? Code { get; set; }
        public string? Title { get; set; }
        public int Credits { get; set; }
        public string? Description { get; set; }
        public string? Instructor { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public int Capacity { get; set; }
        public DateTimeOffset EnrollmentStartDate { get; set; }
        public DateTimeOffset EnrollmentEndDate { get; set; }


    }
}
