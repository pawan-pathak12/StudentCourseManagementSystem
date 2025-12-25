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
        public async Task Create(Student student)
        {
            await _repository.Add(student);
        }



        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            var students = await _repository.GetAll();
            if (!students.Any() || students == null)
            {
                return null;
            }
            return students;
        }

        public async Task<Student?> GetById(int id)
        {
            var student = await _repository.GetById(id);
            if (student == null)
            {
                return null;
            }
            return student;
        }

        public async Task<bool> Update(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return false;
            }
            return await _repository.Update(student);
        }
    }
}
