namespace StudentCourseManagement.Domain.Entities
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public string Description { get; set; }      // Brief summary of the course
        public string Instructor { get; set; }       // Name or ID of the assigned teacher
        public DateTime StartDate { get; set; }      // When the course begins
        public DateTime EndDate { get; set; }        // When the course ends
        public bool IsActive { get; set; }           // For soft delete or visibility

        public int Capacity { get; set; }
        public DateTime EnrollmentStartDate { get; set; }
        public DateTime EnrollmentEndDate { get; set; }
    }
}
