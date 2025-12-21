using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Interfaces.Repositories
{
    public interface IStudentRepository
    {
        void Add(Student student);
        Student? GetById(int id);
        List<Student> GetAll();
        bool Update(Student student);
        bool Delete(int id);
        bool EmailExists(string email);
    }
}
}
