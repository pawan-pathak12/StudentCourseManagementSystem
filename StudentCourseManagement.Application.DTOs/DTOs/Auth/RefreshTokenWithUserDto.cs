using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Application.DTOs.DTOs.Auth
{
    public class RefreshTokenWithUserDto
    {
        public int RefreshTokenId { get; set; }
        public User? User { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRevoked { get; set; }

        //user
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;


    }
}
