using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Interfaces.Services
{
    public interface IEnrollmentService
    {
        #region   CURD Operations 
        Task<bool> CreateAsync(Enrollment enrollment);
        Task<IEnumerable<Enrollment>> GetAllAsync();
        Task<Enrollment?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, Enrollment enrollment);
        Task<bool> DeleteAsync(int id);

        #endregion

    }
}
