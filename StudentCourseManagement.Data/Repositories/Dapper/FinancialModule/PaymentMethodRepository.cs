using Dapper;
using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Data.Database;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Data.Repositories.Dapper.FinancialModule
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly StudentSysDbContext context;
        private readonly ILogger<PaymentMethodRepository> logger;

        public PaymentMethodRepository(StudentSysDbContext context, ILogger<PaymentMethodRepository> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region CURD Operations 

        public async Task<int> AddAsync(PaymentMethod paymentMethod)
        {
            const string sql = @"
                INSERT INTO PaymentMethods (
                    PaymentMethodType, Name, IsActive
                )
                VALUES (
                    @PaymentMethodType, @Name, @IsActive
                );
                SELECT CAST(SCOPE_IDENTITY() as int);"; // SQL Server

            using var connection = context.CreateConnection();

            var newId = await connection.QuerySingleAsync<int>(sql, paymentMethod);

            logger.LogInformation("Successfully added PaymentMethod with ID: {Id}, Type: {MethodType}",
                newId, paymentMethod.PaymentMethodType);

            return newId;
        }

        public async Task<bool> DeleteAsync(int id)
        {

            const string sql = " Update PaymentMethods set IsActive = 0 WHERE PaymentMethodId = @Id";

            using var connection = context.CreateConnection();

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });

            if (affectedRows > 0)
            {
                logger.LogInformation("Successfully softly deleted PaymentMethod with ID: {Id}", id);
                return true;
            }

            logger.LogWarning("PaymentMethod with ID: {Id} not found for deletion", id);
            return false;
        }

        public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
        {

            const string sql = "SELECT * FROM PaymentMethods where IsActive=1 ORDER BY PaymentMethodType ";

            using var connection = context.CreateConnection();

            var methods = await connection.QueryAsync<PaymentMethod>(sql);

            logger.LogInformation("Successfully retrieved {Count} PaymentMethods", methods.Count());

            return methods;
        }

        public async Task<PaymentMethod?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM PaymentMethods WHERE PaymentMethodId = @Id and IsActive=1";

            using var connection = context.CreateConnection();

            var method = await connection.QuerySingleOrDefaultAsync<PaymentMethod>(sql, new { Id = id });

            if (method == null)
            {
                logger.LogWarning("PaymentMethod with ID: {Id} not found", id);
            }
            else
            {
                logger.LogInformation("Successfully retrieved PaymentMethod with ID: {Id}, Type: {MethodType}",
                    id, method.PaymentMethodType);
            }

            return method;
        }

        public async Task<bool> UpdateAsync(int id, PaymentMethod paymentMethod)
        {

            if (id != paymentMethod.PaymentMethodId)
            {
                logger.LogWarning("Update failed: ID mismatch. Provided ID: {ProvidedId}, Entity ID: {EntityId}",
                    id, paymentMethod.PaymentMethodId);
                return false;
            }

            const string sql = @"
                UPDATE PaymentMethods 
                SET PaymentMethodType = @PaymentMethodType,
                    Name = @Name,
                    IsActive = @IsActive
                WHERE PaymentMethodId = @PaymentMethodId";

            using var connection = context.CreateConnection();

            var affectedRows = await connection.ExecuteAsync(sql, paymentMethod);

            if (affectedRows > 0)
            {
                logger.LogInformation("Successfully updated PaymentMethod with ID: {Id}", id);
                return true;
            }

            logger.LogWarning("Update failed  for PaymentMethod ID: {Id}", id);
            return false;
        }
        #endregion

    }
}