using Dapper;
using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Data.Database;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Data.Repositories.Dapper.FinancialModule
{
    public class FeeTemplateRepository : IFeeTemplateRepository
    {
        private readonly StudentSysDbContext context;
        private readonly ILogger<FeeTemplateRepository> logger;

        public FeeTemplateRepository(StudentSysDbContext context, ILogger<FeeTemplateRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<int> AddAsync(FeeTemplate feeTemplate)
        {
            logger.LogInformation("Repo : Adding new FeeTemplate: {Name}, CourseId: {CourseId}, Amount: {Amount}",
                feeTemplate.Name, feeTemplate.CourseId, feeTemplate.Amount);

            const string sql = @"
                INSERT INTO FeeTemplates (
                    Name, CourseId, CalculationType, Amount, RatePerCredit, 
                    IsActive, CreatedAt, UpdatedAt
                )
                VALUES (
                    @Name, @CourseId, @CalculationType, @Amount, @RatePerCredit,
                    @IsActive, @CreatedAt, @UpdatedAt
                );
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = context.CreateConnection();

            var newId = await connection.QuerySingleAsync<int>(sql, feeTemplate);

            logger.LogInformation("Successfully added FeeTemplate with ID: {Id}", newId);

            return newId;
        }

        public async Task<bool> DeleteAsync(int id)
        {

            const string sql = "Update FeeTemplates set IsActive =0 WHERE FeeTemplateId = @Id";

            using var connection = context.CreateConnection();

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });

            if (affectedRows > 0)
            {
                logger.LogInformation("Successfully softly deleted FeeTemplate with ID: {Id}", id);
                return true;
            }

            logger.LogWarning("FeeTemplate with ID: {Id} not found for deletion", id);
            return false;
        }

        public async Task<IEnumerable<FeeTemplate>> GetAllAsync()
        {

            const string sql = @"Select * from FeeTemplates where IsActive=1";


            using var connection = context.CreateConnection();

            var templates = await connection.QueryAsync<FeeTemplate>(sql);

            logger.LogInformation("Successfully retrieved {Count} FeeTemplates", templates.Count());

            return templates;
        }

        public async Task<FeeTemplate?> GetByIdAsync(int id)
        {
            const string sql = @"Select * from FeeTemplates where IsActive =1 and FeeTemplateId=@Id";

            using var connection = context.CreateConnection();

            var template = await connection.QuerySingleOrDefaultAsync<FeeTemplate>(sql, new { Id = id });

            if (template == null)
            {
                logger.LogWarning("FeeTemplate with ID: {Id} not found", id);
            }
            else
            {
                logger.LogInformation("Successfully retrieved FeeTemplate with ID: {Id}", id);
            }

            return template;
        }

        public async Task<bool> UpdateAsync(int id, FeeTemplate feeTemplate)
        {
            if (id != feeTemplate.FeeTemplateId)
            {
                logger.LogWarning("Update failed: ID mismatch. Provided ID: {ProvidedId}, Entity ID: {EntityId}",
                    id, feeTemplate.FeeTemplateId);
                return false;
            }

            const string sql = @"
                UPDATE FeeTemplates 
                SET Name = @Name,
                    CourseId = @CourseId,
                    CalculationType = @CalculationType,
                    Amount = @Amount,
                    RatePerCredit = @RatePerCredit,
                    IsActive = @IsActive,
                    UpdatedAt = @UpdatedAt
                WHERE FeeTemplateId = @FeeTemplateId";

            using var connection = context.CreateConnection();

            var affectedRows = await connection.ExecuteAsync(sql, feeTemplate);

            if (affectedRows > 0)
            {
                logger.LogInformation("Successfully updated FeeTemplate with ID: {Id}", id);
                return true;
            }

            logger.LogWarning("No rows updated for FeeTemplate ID: {Id} (possibly not found or no changes)", id);
            return false;
        }
    }
}