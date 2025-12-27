using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Interfaces.Services
{
    public interface IStudentService
    {
        Task<bool> CreateAsync(Student student);
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, Student student);
        Task<bool> DeleteAsync(int id);
    }
}
