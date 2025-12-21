using StudentCourseManagement.Business.DTOs.Student;

namespace StudentCourseManagement.Business.Interfaces.Services
{
    public interface IStudentService
    {
        bool Create(CreateStudentDto dto);
        List<StudentResponseDto> GetAll();
        StudentResponseDto? GetById(int id);
        bool Update(int id, UpdateStudentDto dto);
        bool Delete(int id);
    }
}
