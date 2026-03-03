using System.ComponentModel.DataAnnotations;

namespace StudentCourseManagement.Application.DTOs.Students
{
    public class UpdateStudentDto
    {
        public int StudentId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        public string? Address { get; set; }
        [Required]
        public DateTimeOffset DOB { get; set; }
        [Required]
        [Range(1000000000, long.MaxValue, ErrorMessage = "Number must be at least 10 digits.")]
        public long Number { get; set; }
        public string? Gender { get; set; }
        public bool IsActive { get; set; }
    }
}
