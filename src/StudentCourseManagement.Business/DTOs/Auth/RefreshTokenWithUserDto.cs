using StudentCourseManagement.Domain.Entities.Identites;

namespace StudentCourseManagement.Application.DTOs.Auth
{
    public class RefreshTokenWithUserDto
    {
        public int RefreshTokenId { get; set; }
        public User? User { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool IsRevoked { get; set; }

        //user
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;


    }
}
