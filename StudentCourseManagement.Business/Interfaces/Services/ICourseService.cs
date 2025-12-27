using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Interfaces.Services
{
    public interface ICourseService
    {
        Task<int> CreateAsync(Course course);
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, Course course);
        Task<bool> DeleteAsync(int id);
    }
}
