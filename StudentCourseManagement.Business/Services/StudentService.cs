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
        public async Task<bool> CreateAsync(Student student)
        {
            var emailExists = await _repository.EmailExistsAsync(student.Email);
            if (emailExists)
            {
                return false;
            }
            await _repository.AddAsync(student);
            return true;
        }



        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            var students = await _repository.GetAllAsync();
            if (!students.Any() || students == null)
            {
                return null;
            }
            return students;
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
            {
                return null;
            }
            return student;
        }

        public async Task<bool> UpdateAsync(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return false;
            }
            var isStudentActive = await _repository.IsStudentActiveAsync(id);
            if (!isStudentActive)
            {
                return false;
            }
            return await _repository.UpdateAsync(student);
        }
    }
}
