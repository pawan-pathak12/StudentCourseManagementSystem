using StudentCourseManagement.Application.DTOs.Auth;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Interfaces.Repositories.Identities
{
    public interface IRefreshTokenRepository
    {
        Task<int> AddAsync(RefreshToken refreshToken);
        Task<RefreshTokenWithUserDto?> GetRefreshTokenWithUserAsync(string token);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<int> UpdateAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetByIdAsync(int refreshTokenId);
    }
}
