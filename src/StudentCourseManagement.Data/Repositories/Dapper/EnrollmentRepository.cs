using Dapper;
using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Data.Database;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Data.Repositories.Dapper
{

    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly StudentSysDbContext _dbContext;
        private readonly ILogger<EnrollmentRepository> _logger;

        public EnrollmentRepository(StudentSysDbContext dbContext, ILogger<EnrollmentRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        #region CURD Operations 
        // CREATE - Returns the new EnrollmentId
        public async Task<int> AddAsync(Enrollment enrollment)
        {
            const string sql = @"
                    INSERT INTO Enrollments (StudentId, CourseId, EnrollmentStatus, EnrollmentDate, IsActive, FeeAssessmentDate, CancelledDate, CancellationReason)
                    VALUES (@StudentId, @CourseId, @EnrollmentStatus, @EnrollmentDate, @IsActive,   @FeeAssessmentDate, @CancelledDate, @CancellationReason);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = _dbContext.CreateConnection();
            var newId = await connection.QuerySingleOrDefaultAsync<int>(sql, enrollment);

            if (newId > 0)
            {
                _logger.LogInformation("Repo: Enrollment added successfully with ID {EnrollmentId} for StudentId {StudentId} and CourseId {CourseId}",
                    newId, enrollment.StudentId, enrollment.CourseId);
                return newId;
            }

            _logger.LogWarning("Repo: Enrollment creation failed for StudentId {StudentId} and CourseId {CourseId}",
                enrollment.StudentId, enrollment.CourseId);
            return 0;
        }

        // GET ALL - Only active enrollments
        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            const string sql = "Select * from Enrollments where ISActive=1";

            using var connection = _dbContext.CreateConnection();
            var enrollments = await connection.QueryAsync<Enrollment>(sql);

            _logger.LogInformation("Repo: Fetching  Active Enrollment , total count = " + enrollments.Count());

            return enrollments;
        }

        // GET BY ID
        public async Task<Enrollment?> GetByIdAsync(int id)
        {
            const string sql = @"Select * from Enrollments where EnrollmentId =@Id and IsActive=1";


            using var connection = _dbContext.CreateConnection();
            var enrollment = await connection.QueryFirstOrDefaultAsync<Enrollment?>(sql, new { Id = id });

            if (enrollment == null)
            {
                _logger.LogWarning("Repo: Enrollment with ID {EnrollmentId} not found or inactive", id);
            }
            else
            {
                _logger.LogInformation("Repo: Enrollment retrieved with ID {EnrollmentId}", id);
            }

            return enrollment;
        }

        // UPDATE
        public async Task<bool> UpdateAsync(int id, Enrollment enrollment)
        {

            enrollment.EnrollmentId = id;

            const string sql = @"
                UPDATE Enrollments
                SET 
                    StudentId = @StudentId,
                    CourseId = @CourseId,
                    EnrollmentStatus = @EnrollmentStatus,
                    EnrollmentDate = @EnrollmentDate,
                    IsActive = @IsActive,
                    FeeAssessmentDate = @FeeAssessmentDate,
                    CancelledDate = @CancelledDate,
                    CancellationReason = @CancellationReason
                WHERE EnrollmentId = @EnrollmentId AND IsActive = 1;";


            using var connection = _dbContext.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, enrollment);

            if (rowsAffected > 0)
            {
                _logger.LogInformation("Repo: Enrollment with ID {EnrollmentId} updated successfully", id);
            }
            else
            {
                _logger.LogWarning("Repo: Enrollment with ID {EnrollmentId} not found or already inactive during update", id);
            }
            return rowsAffected > 0;
        }

        // DELETE - Soft delete by setting IsActive = false
        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = @"
                UPDATE Enrollments SET IsActive = 0
                WHERE EnrollmentId = @Id AND IsActive = 1;";


            using var connection = _dbContext.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });

            if (rowsAffected > 0)
            {
                _logger.LogInformation("Repo: Enrollment with ID {EnrollmentId} soft-deleted successfully", id);
            }
            else
            {
                _logger.LogWarning("Repo: Enrollment with ID {EnrollmentId} not found or already inactive during delete", id);
            }

            return rowsAffected > 0;

        }

        #endregion


        #region Business Logc required method

        public async Task<bool> ExistsAsync(int studentId, int courseId)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"select case 
                           when exists (
                               select 1 from Enrollments 
                               where StudentId = @StudentId and CourseId = @CourseId
                           ) 
                           then 1 else 0 end";

            _logger.LogInformation(
                "Checking if enrollment exists for StudentId {StudentId} in CourseId {CourseId}",
                studentId, courseId);

            var result = await connection.ExecuteScalarAsync<int>(sql, new { StudentId = studentId, CourseId = courseId });
            return result == 1;
        }

        public async Task<int> GetEnrollmentCountByCourse(int courseId)
        {

            using var connection = _dbContext.CreateConnection();
            const string sql = @"select count(1) from Enrollments where CourseId =@CourseId ;";

            _logger.LogInformation("Getting enrollment count for CourseId {CourseId}", courseId);

            var count = await connection.ExecuteScalarAsync<int>(sql, new { CourseId = courseId });

            return count;
        }

        public async Task<int> GetEnrollmentCountByStudent(int studentId)
        {

            using var connection = _dbContext.CreateConnection();
            const string sql = @"select count(1) from Enrollments where StudentId =@StudentId ;";

            _logger.LogInformation("Getting enrollment count for StudentId {StudentId}", studentId);

            var count = await connection.ExecuteScalarAsync<int>(sql, new { StudentId = studentId });

            return count;
        }
        #endregion

        public async Task<bool> UpdateFeeAssessedDateAsync(int enrollmentId)
        {
            var connection = _dbContext.CreateConnection();
            var query = "Update Enrollments set FeeAssessmentDate=@Date where EnrollmentId=@Id";
            var rowsAffected = await connection.ExecuteAsync(query, new { Id = enrollmentId, Date = DateTime.UtcNow });
            if (rowsAffected > 0)
            {
                _logger.LogInformation("Repo: Enrollment FeeAssessmnet Date with ID {EnrollmentId} updated successfully", enrollmentId);
            }
            else
            {
                _logger.LogWarning("Repo: Enrollment with ID {EnrollmentId} not found or already inactive during update", enrollmentId);
            }
            return rowsAffected > 0;
        }


    }
}