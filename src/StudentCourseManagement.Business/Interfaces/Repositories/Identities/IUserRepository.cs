using StudentCourseManagement.Domain.Entities.Identites;

namespace StudentCourseManagement.Business.Interfaces.Repositories.Identities
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
