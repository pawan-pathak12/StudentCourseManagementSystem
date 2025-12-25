using System.ComponentModel.DataAnnotations;

namespace StudentCourseManagement.Business.DTOs.Student
{
    public class CreateStudentDto
    {
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "DOB is required")]
        public DateTimeOffset DOB { get; set; }
        public long Number { get; set; }
        public string Gender { get; set; }


    }
}
