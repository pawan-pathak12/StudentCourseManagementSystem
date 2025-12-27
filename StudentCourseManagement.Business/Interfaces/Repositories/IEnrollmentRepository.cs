using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Interfaces.Repositories
{
    public interface IEnrollmentRepository
    {
        #region CURD Operation
        Task<int> AddAsync(Enrollment enrollment);
        Task<IEnumerable<Enrollment>> GetAllAsync();
        Task<Enrollment?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, Enrollment enrollment);
        Task<bool> DeleteAsync(int id);
        #endregion
    }
}
