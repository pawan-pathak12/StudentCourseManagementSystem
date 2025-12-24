namespace StudentCourseManagement.Business.DTOs.Student
{
    public class CreateStudentDto
    {
        public string Name { get; set; }
        public string Email { get; set; } = "";
        public string Address { get; set; }

    }
}
