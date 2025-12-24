using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Interfaces.Repositories
{
    public interface IStudentRepository
    {
        #region CURD Operations 
        Task Add(Student student);
        Task<Student> GetById(int id);
        Task<IEnumerable<Student>> GetAll();
        Task<bool> Update(Student student);
        Task<bool> Delete(int id);

        #endregion

        bool EmailExists(string email);
    }
}

