namespace StudentCourseManagement.Business.DTOs.Enrollments
{
    public class CreateEnrollmentDto
    {
        public int StudentId { get; set; }          // FK to Student
        public int CourseId { get; set; }           // FK to Course


    }
}
