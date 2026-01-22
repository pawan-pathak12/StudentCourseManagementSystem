using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Interfaces.Repositories
{
    public interface ICourseRepository
    {
        #region CURD Operations 
        Task<int> AddAsync(Course course);
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, Course course);
        Task<bool> DeleteAsync(int id);

        #endregion

        #region  logic validation  of Course
        Task<bool> CodeExistsAsync(string code);
        Task<bool> TitleExistsAsync(string Name);

        #endregion

        #region Enrollment Service Required Methods 
        Task<bool> CheckEnrollmentDateAsync(int courseId, DateTimeOffset enrollmentDate);

        #endregion

    }
}
