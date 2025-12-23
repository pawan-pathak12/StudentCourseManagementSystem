using StudentCourseManagement.Business.DTOs.Student;

namespace StudentCourseManagement.Business.Interfaces.Services
{
    public interface IStudentService
    {
        Task<bool> Create(CreateStudentDto dto);
        Task<List<StudentResponseDto>> GetAll();
        Task<StudentResponseDto?> GetById(int id);
        Task<bool> Update(int id, UpdateStudentDto dto);
        Task<bool> Delete(int id);
    }
}
