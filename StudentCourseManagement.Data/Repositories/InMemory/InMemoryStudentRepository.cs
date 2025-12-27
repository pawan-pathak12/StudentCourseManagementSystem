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

        #region CURD Operations 
        public Task<int> AddAsync(Student student)
        {
            _students.Add(student);

            student.StudentId++;
            return Task.FromResult(student.StudentId);
        }

        public Task<IEnumerable<Student>> GetAllAsync()
        {
            return Task.FromResult(_students.AsEnumerable());
        }

        public Task<Student> GetByIdAsync(int id)
        {
            var result = _students.Find(x => x.StudentId == id);
            return Task.FromResult(result);

        }

        public Task<bool> UpdateAsync(Student student)
        {
            var existingStudent = _students.Find(x => x.StudentId == student.StudentId);
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

        public Task<bool> DeleteAsync(int id)
        {

            var existingStudent = _students.FirstOrDefault(x => x.StudentId == id);
            if (existingStudent == null)
            {
                return Task.FromResult(false);
            }
            existingStudent.IsActive = false;
            return Task.FromResult(true);

        }
        #endregion

        #region Student busines logic 
        public Task<bool> EmailExistsAsync(string email)
        {
            var emailExist = _students.Find(x => x.Email == email);
            if (emailExist == null)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public Task<bool> IsStudentActiveAsync(int studentId)
        {
            var student = _students.Find(x => x.StudentId == studentId);
            if (!student.IsActive)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);

        }

        #endregion

    }
}
