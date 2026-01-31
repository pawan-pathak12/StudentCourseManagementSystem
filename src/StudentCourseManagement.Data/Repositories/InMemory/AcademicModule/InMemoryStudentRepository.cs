using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Data.Repositories.InMemory.AcademicModule
{
    public class InMemoryStudentRepository : IStudentRepository
    {
        private readonly InMemoryDbContext _db;
        private readonly List<Student> _students;

        public InMemoryStudentRepository(InMemoryDbContext db)
        {
            this._db = db;
            _students = _db.Students;
        }

        #region CURD Operations 
        public Task<int> AddAsync(Student student)
        {
            var newId = _students.Count + 1;
            student.StudentId = newId;
            _students.Add(student);
            return Task.FromResult(newId);
        }

        public Task<IEnumerable<Student>> GetAllAsync()
        {
            return Task.FromResult(_students.AsEnumerable());
        }

        public Task<Student> GetByIdAsync(int id)
        {
            var result = _students.Find(x => x.StudentId == id && x.IsActive == true);
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
