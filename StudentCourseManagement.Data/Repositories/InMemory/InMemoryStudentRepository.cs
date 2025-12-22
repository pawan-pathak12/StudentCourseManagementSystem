using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Data.Repositories.InMemory
{
    public class InMemoryStudentRepository : IStudentRepository
    {
        private readonly List<Student> _students;
        private int _id = 1;
        public InMemoryStudentRepository()
        {
            _students = new List<Student>();
        }
        public void Add(Student student)
        {
            student.Id = _id++;
            _students.Add(student);
        }

        public Student? GetById(int id)
           => _students.FirstOrDefault(s => s.Id == id);

        public List<Student> GetAll()
            => _students;

        public bool Update(Student student)
            => true;

        public bool Delete(int id)
        {
            var s = GetById(id);
            if (s == null) return false;
            _students.Remove(s);
            return true;
        }

        public bool EmailExists(string email)
            => _students.Any(s => s.Email == email);
    }
}
