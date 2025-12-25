namespace StudentCourseManagement.Domain.Entities
{
    public class Student
    {

        public int StudentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTimeOffset DOB { get; set; }
        public string Number { get; set; }
        public DateTimeOffset EnrollmentDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string Gender { get; set; }
        public string Address { get; set; }

        //  for now CourseIds is not use 
        //public List<int> CourseIds { get; set; }
    }

}
