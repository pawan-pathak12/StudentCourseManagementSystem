using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Interfaces.Services
{
    public interface ICourseService
    {
        Task<int> Create(Course course);
        Task<IEnumerable<Course>> GetAll();
        Task<Course?> GetById(int id);
        Task<bool> Update(int id, Course course);
        Task<bool> Delete(int id);
    }
}
