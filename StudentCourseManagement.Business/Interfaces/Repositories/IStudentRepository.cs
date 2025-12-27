using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Interfaces.Repositories
{
    public interface IStudentRepository
    {
        #region CURD Operations 
        Task<int> AddAsync(Student student);
        Task<Student> GetByIdAsync(int id);
        Task<IEnumerable<Student>> GetAllAsync();
        Task<bool> UpdateAsync(Student student);
        Task<bool> DeleteAsync(int id);

        #endregion

        #region Validation 

        #region Student business logic 
        Task<bool> IsStudentActiveAsync(int studentId);
        Task<bool> EmailExistsAsync(string email);

        #endregion


        #endregion
    }
}

