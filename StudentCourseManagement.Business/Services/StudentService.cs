using StudentCourseManagement.Business.DTOs.Student;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public bool Create(CreateStudentDto dto)
        {
            /* if (_repository.EmailExists(dto.Email))
                 return false;
 */
            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email
            };

            _repository.Add(student);
            return true;
        }

        public List<StudentResponseDto> GetAll()
        {
            return _repository.GetAll()
                .Select(s => new StudentResponseDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email
                }).ToList();
        }

        public StudentResponseDto? GetById(int id)
        {
            var student = _repository.GetById(id);
            if (student == null) return null;

            return new StudentResponseDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email
            };
        }

        public bool Update(int id, UpdateStudentDto dto)
        {
            var student = _repository.GetById(id);
            if (student == null) return false;

            student.Name = dto.Name;
            student.Email = dto.Email;

            return _repository.Update(student);
        }

        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }
    }
}
