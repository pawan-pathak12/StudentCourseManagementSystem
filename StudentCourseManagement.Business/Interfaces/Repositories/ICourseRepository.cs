using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Interfaces.Repositories
{
    public interface ICourseRepository
    {
        #region CURD Operations 
        Task<int> CreateAsync(Course course);
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, Course course);
        Task<bool> DeleteAsync(int id);
        #endregion
    }
}
