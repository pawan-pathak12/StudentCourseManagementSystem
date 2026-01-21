using StudentCourseManagement.Application.DTOs.DTOs.Auth;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Interfaces.Repositories.Identity
{
    public interface IRefreshTokenRepository
    {
        Task<int> AddAsync(RefreshToken refreshToken);
        Task<RefreshTokenWithUserDto?> GetRefreshTokenWithUserAsync(string token);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<int> UpdateAsync(RefreshToken refreshToken);
    }
}
