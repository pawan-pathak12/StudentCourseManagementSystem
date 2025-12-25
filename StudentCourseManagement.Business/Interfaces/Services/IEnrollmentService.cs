using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Interfaces.Services
{
    public interface IEnrollmentService
    {
        #region   CURD Operations 
        Task<int> CreateEnrollmentAsync(Enrollment enrollment);
        Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync();
        Task<Enrollment?> GetEnrollmentByIdAsync(int id);
        Task<bool> UpdateEnrollmentAsync(int id, Enrollment enrollment);
        Task<bool> DeleteEnrollmentAsync(int id);

        #endregion
    }
}
