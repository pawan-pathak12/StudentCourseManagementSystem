using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Interfaces.Repositories
{
    public interface IEnrollmentRepository
    {
        #region CURD Operation
        Task<int> Create(Enrollment enrollment);
        Task<IEnumerable<Enrollment>> GetAll();
        Task<Enrollment?> GetById(int id);
        Task<bool> Update(int id, Enrollment enrollment);
        Task<bool> Delete(int id);
        #endregion
    }
}
