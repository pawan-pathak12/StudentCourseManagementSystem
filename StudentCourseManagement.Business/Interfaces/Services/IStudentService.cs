using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Interfaces.Services
{
    public interface IStudentService
    {
        Task Create(Student student);
        Task<List<Student>> GetAll();
        Task<Student?> GetById(int id);
        Task<bool> Update(int id, Student student);
        Task<bool> Delete(int id);
    }
}
