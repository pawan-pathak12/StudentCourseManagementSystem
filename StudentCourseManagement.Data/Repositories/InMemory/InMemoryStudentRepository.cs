using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Data.Repositories.InMemory
{
    public class InMemoryStudentRepository : IStudentRepository
    {
        public readonly List<Student> _students;
        public InMemoryStudentRepository()
        {
            _students = new List<Student>();
        }


        public Task Add(Student student)
        {
            _students.Add(student);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Student>> GetAll()
        {
            return Task.FromResult(_students.AsEnumerable());
        }

        public Task<Student> GetById(int id)
        {
            var result = _students.Find(x => x.Id == id);
            return Task.FromResult(result);

        }

        public Task<bool> Update(Student student)
        {
            var existingStudent = _students.Find(x => x.Id == student.Id);
            if (existingStudent == null)
            {
                return Task.FromResult(false);
            }


            existingStudent.Name = student.Name;
            existingStudent.Email = student.Email;
            existingStudent.DOB = student.DOB;
            existingStudent.Number = student.Number;
            existingStudent.Gender = student.Gender;
            existingStudent.Address = student.Address;

            return Task.FromResult(true);

        }

        public Task<bool> Delete(int id)
        {

            var existingStudent = _students.FirstOrDefault(x => x.Id == id);
            if (existingStudent == null)
            {
                return Task.FromResult(false);
            }
            existingStudent.IsActive = false;
            return Task.FromResult(true);

        }
        public bool EmailExists(string email)
        {
            throw new NotImplementedException();
        }
    }
}
