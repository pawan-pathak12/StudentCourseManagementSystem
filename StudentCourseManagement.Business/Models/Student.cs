namespace Student_Course_Management_API.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string Number { get; set; }

        // New Additions 👇
        public DateTime EnrollmentDate { get; set; }    // When student joined
        public bool IsActive => true;            // For soft delete or status tracking
        public string Gender { get; set; }              // Optional: "Male", "Female", "Other"
        public string Address { get; set; }             // Optional: for communication

        //  for now CourseIds is not use 
        //public List<int> CourseIds { get; set; }

    }

}
