using StudentCourseManagement.Business.DTOs.Student;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Services;

namespace StudentCourseManagement.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Create(CreateStudentDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<StudentResponseDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<StudentResponseDto?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(int id, UpdateStudentDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
