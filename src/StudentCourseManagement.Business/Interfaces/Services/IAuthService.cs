using StudentCourseManagement.Domain.Entities.Identites;

namespace StudentCourseManagement.Business.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
    }
}
