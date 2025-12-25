using System.ComponentModel.DataAnnotations;

namespace StudentCourseManagement.Business.DTOs.Student
{
    public class UpdateStudentDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        public string Address { get; set; }
        [Required]
        public DateTimeOffset DOB { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "Number must be at least 10 digits.")]
        public long Number { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
    }
}
