using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Data.Repositories.InMemory
{
    public class InMemoryStudentRepository : IStudentRepository
    {
        private readonly List<Student> _students;
        private int _id = 1;

        public Task Add(Student student)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool EmailExists(string email)
        {
            throw new NotImplementedException();
        }

        public Task<List<Student>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Student?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Student student)
        {
            throw new NotImplementedException();
        }
    }
}
