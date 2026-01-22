using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
    }
}
