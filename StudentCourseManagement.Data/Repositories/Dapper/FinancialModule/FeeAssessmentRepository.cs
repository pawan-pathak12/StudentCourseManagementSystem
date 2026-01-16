using Dapper;
using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Data.Database;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Data.Repositories.Dapper.FinancialModule
{


    public class FeeAssessmentRepository : IFeeAssessmentRepository
    {
        private readonly StudentSysDbContext _context;
        private readonly ILogger<FeeAssessmentRepository> _logger;

        public FeeAssessmentRepository(StudentSysDbContext context, ILogger<FeeAssessmentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region CURD Operation
        public async Task<int> AddAsync(FeeAssessment feeAssessment)
        {
            _logger.LogInformation("Adding new FeeAssessment for EnrollmentId: {EnrollmentId}, Amount: {Amount}",
                feeAssessment.EnrollmentId, feeAssessment.Amount);

            const string sql = @"
            INSERT INTO FeeAssessments (
                EnrollmentId, CourseId, FeeTemplateId, Amount, 
                DueDate, FeeAssessmentStatus, IsActive, PaidDate, LateFeeAmount, LateFeeAppliedDate
            )
            VALUES (
                @EnrollmentId, @CourseId, @FeeTemplateId, @Amount,
                @DueDate, @FeeAssessmentStatus, @IsActive, @PaidDate, @LateFeeAmount, @LateFeeAppliedDate
            );
            SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = _context.CreateConnection();

            var id = await connection.QuerySingleAsync<int>(sql, feeAssessment);
            if (id == 0 || id == null)
            {
                _logger.LogWarning("Error adding FeeAssessment for EnrollmentId: {EnrollmentId}", feeAssessment.EnrollmentId);

            }
            return id;


        }

        public async Task<bool> DeleteAsync(int id)
        {

            const string sql = "Update FeeAssessments set IsActive=0 WHERE FeeAssessmentId = @Id";

            using var connection = _context.CreateConnection();


            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            bool success = affectedRows > 0;

            if (success)
            {
                _logger.LogInformation("Successfully deleted FeeAssessment with ID: {Id}", id);
            }
            else
            {
                _logger.LogWarning("FeeAssessment with ID: {Id} not found for deletion", id);
            }
            return success;

        }

        public async Task<IEnumerable<FeeAssessment>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all FeeAssessments");

            const string sql = "SELECT * FROM FeeAssessments where IsActive=1";

            using var connection = _context.CreateConnection();

            var assessments = await connection.QueryAsync<FeeAssessment>(sql);
            return assessments;



        }

        public async Task<FeeAssessment?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving FeeAssessment with ID: {Id}", id);

            const string sql = "SELECT * FROM FeeAssessments WHERE FeeAssessmentId = @Id and IsActive=1";

            using var connection = _context.CreateConnection();


            var feeAssessment = await connection.QueryFirstOrDefaultAsync<FeeAssessment>(sql, new { Id = id });

            if (feeAssessment == null)
            {
                _logger.LogWarning("FeeAssessment with ID: {Id} not found", id);
            }
            else
            {
                _logger.LogError("Error retrieving FeeAssessment with ID: {Id}", id);
            }

            return feeAssessment;
        }


        public async Task<bool> UpdateAsync(int id, FeeAssessment feeAssessment)
        {
            _logger.LogInformation("Updating FeeAssessment with ID: {Id}", id);

            if (id != feeAssessment.FeeAssessmentId)
            {
                _logger.LogWarning("Update failed: ID mismatch. Provided ID: {ProvidedId}, Entity ID: {EntityId}", id, feeAssessment.FeeAssessmentId);
                return false;
            }

            const string sql = @"
            UPDATE FeeAssessments 
            SET EnrollmentId = @EnrollmentId,
                CourseId = @CourseId,
                FeeTemplateId = @FeeTemplateId,
                Amount = @Amount,
                DueDate = @DueDate,
                FeeAssessmentStatus = @FeeAssessmentStatus,
                IsActive = @IsActive,
                PaidDate = @PaidDate,
                LateFeeAmount = @LateFeeAmount,
                LateFeeAppliedDate = @LateFeeAppliedDate
            WHERE FeeAssessmentId = @FeeAssessmentId";

            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(sql, feeAssessment);
            bool success = affectedRows > 0;

            if (success)
            {
                _logger.LogInformation("Successfully updated FeeAssessment with ID: {Id}", id);
            }
            else
            {
                _logger.LogError("Error updating FeeAssessment with ID: {Id}", id);
            }

            return success;


        }

        #endregion

        #region Phase -3 required method 
        public async Task<bool> ExistsByEnrollmentIdAsync(int enrollmentId)
        {
            const string sql = @"SELECT CASE WHEN EXISTS (
                                SELECT 1 
                                FROM FeeAssessments 
                                WHERE EnrollmentId = @EnrollmentId
                            ) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END;";

            using var connection = _context.CreateConnection();

            var result = await connection.ExecuteScalarAsync<bool>(sql, new { EnrollmentId = enrollmentId });

            if (!result)
            {
                _logger.LogWarning("FeeAssessment with Enrollment ID: {Id} not found", enrollmentId);
            }
            else
            {
                _logger.LogInformation("Successfully retrieved FeeAssessment with Enrollment Id : {Id}", enrollmentId);
            }

            return result;
        }

        public async Task<FeeAssessment> GetByEnrolmentIdAsync(int enrollmentId)
        {
            _logger.LogInformation("Retrieving FeeAssessment with Enrollment ID: {Id}", enrollmentId);

            const string sql = "SELECT * FROM FeeAssessments WHERE EnrollmentId = @Id and IsActive=1";

            using var connection = _context.CreateConnection();


            var feeAssessment = await connection.QueryFirstOrDefaultAsync<FeeAssessment>(sql, new { Id = enrollmentId });

            if (feeAssessment == null)
            {
                _logger.LogWarning("FeeAssessment with Enrollment ID: {Id} not found", enrollmentId);
            }
            else
            {
                _logger.LogError("Error retrieving FeeAssessment with Enrollment ID: {Id}", enrollmentId);
            }

            return feeAssessment;
        }
        #endregion

    }
}
