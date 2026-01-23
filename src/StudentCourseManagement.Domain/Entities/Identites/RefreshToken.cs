namespace StudentCourseManagement.Domain.Entities.Identites
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }
        public string? Token { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? RevokedAt { get; set; }
        public bool IsRevoked { get; set; }
        public string? ReplacedByToken { get; set; }
    }
}
