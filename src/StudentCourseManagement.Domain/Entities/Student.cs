namespace StudentCourseManagement.Domain.Entities
{
    public class Student
    {

        public int StudentId { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public DateTimeOffset DOB { get; set; }
        public long Number { get; set; }
        public DateTimeOffset EnrollmentDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string Gender { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        //  for now CourseIds is not use 
        //public List<int> CourseIds { get; set; }
    }

}
