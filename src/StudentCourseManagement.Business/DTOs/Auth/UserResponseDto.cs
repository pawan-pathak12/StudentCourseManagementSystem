namespace StudentCourseManagement.Business.DTOs.Auth
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
