using System.ComponentModel.DataAnnotations;

namespace StudentCourseManagement.Application.DTOs.DTOs.Students
{
    public class CreateStudentDto
    {
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "DOB is required")]
        public DateTimeOffset DOB { get; set; }
        public long Number { get; set; }
        public string Gender { get; set; } = string.Empty;


    }
}
