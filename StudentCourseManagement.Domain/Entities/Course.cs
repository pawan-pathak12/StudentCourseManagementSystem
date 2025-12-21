namespace Student_Course_Management_API.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }

        // 🔄 New Additions 👇
        public string Description { get; set; }      // Brief summary of the course
        public string Instructor { get; set; }       // Name or ID of the assigned teacher
        public DateTime StartDate { get; set; }      // When the course begins
        public DateTime EndDate { get; set; }        // When the course ends
        public bool IsActive { get; set; }           // For soft delete or visibility

        public int Capacity { get; set; }

        // New Addtions -2
        public DateTime EnrollmentStartDate { get; set; }
        public DateTime EnrollmentEndDate { get; set; }

        #region Later to add 
        //  public int MaxEnrollment { get; set; }       // Capacity limit

        //  public List<int> EnrolledStudentIds { get; set; } // Optional: student IDs enrolled
        #endregion


    }
}
