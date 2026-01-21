using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Interfaces.Repositories.Identity
{
    public interface IUserRepository
    {
        Task<int> AddAsync(User user);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<User?> GetByEmailAddressAsync(string? email);

        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int userId);
    }
}
