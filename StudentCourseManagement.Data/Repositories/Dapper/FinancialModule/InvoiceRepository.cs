using Dapper;
using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Data.Database;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Data.Repositories.Dapper.FinancialModule
{
    public class InvoiceRepository : IInvoiceRepository
    {

        private readonly StudentSysDbContext context;
        private readonly ILogger<InvoiceRepository> logger;

        public InvoiceRepository(StudentSysDbContext context, ILogger<InvoiceRepository> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #region CURD Operations

        public async Task<int> AddAsync(Invoice invoice)
        {
            const string sql = @"
                INSERT INTO Invoices (
                    InvoiceNumber, StudentId, CourseId, FeeAssessmentId, LateFeeApplied, IssuedAt, DueDate, TotalAmount, InvoiceStatus, CreatedAt,
                    AmountPaid, BalanceDue, UpdatedAt, Discount) VALUES
                    (
                    @InvoiceNumber, @StudentId, @CourseId, @FeeAssessmentId, @LateFeeApplied, @IssuedAt, @DueDate, @TotalAmount, @InvoiceStatus, @CreatedAt,
                    @AmountPaid, @BalanceDue, @UpdatedAt, @Discount
                );
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = context.CreateConnection();

            var newId = await connection.QuerySingleAsync<int>(sql, invoice);

            logger.LogInformation("Successfully added Invoice with ID: {Id} and InvoiceNumber: {InvoiceNumber}",
                newId, invoice.InvoiceNumber);

            return newId;
        }

        public async Task<bool> DeleteAsync(int id)
        {

            const string sql = "Update Invoices set IsActive =0 WHERE InvoiceId = @Id";

            using var connection = context.CreateConnection();

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });

            if (affectedRows > 0)
            {
                logger.LogInformation("Successfully deleted Invoice with ID: {Id}", id);
                return true;
            }

            logger.LogWarning("Invoice with ID: {Id} not found for deletion", id);
            return false;
        }

        public async Task<IEnumerable<Invoice>> GetAllAsync()
        {

            const string sql = "SELECT * FROM Invoices where IsActive=1";

            using var connection = context.CreateConnection();

            var invoices = await connection.QueryAsync<Invoice>(sql);

            logger.LogInformation("Successfully retrieved {Count} Invoices", invoices.Count());

            return invoices;
        }

        public async Task<Invoice?> GetByIdAsync(int id)
        {

            const string sql = "SELECT * FROM Invoices WHERE IsActive =1 and  InvoiceId = @Id";

            using var connection = context.CreateConnection();

            var invoice = await connection.QuerySingleOrDefaultAsync<Invoice>(sql, new { Id = id });

            if (invoice == null)
            {
                logger.LogWarning("Invoice with ID: {Id} not found", id);
            }
            else
            {
                logger.LogInformation("Successfully retrieved Invoice with ID: {Id} and InvoiceNumber: {InvoiceNumber}",
                    id, invoice.InvoiceNumber);
            }

            return invoice;
        }

        public async Task<bool> UpdateAsync(int id, Invoice invoice)
        {

            const string sql = @"
                UPDATE Invoices 
                SET InvoiceNumber = @InvoiceNumber,
                    StudentId = @StudentId,
                    CourseId = @CourseId,
                    FeeAssessmentId = @FeeAssessmentId,
                    LateFeeApplied = @LateFeeApplied,
                    IssuedAt = @IssuedAt,
                    DueDate = @DueDate,
                    TotalAmount = @TotalAmount,
                    InvoiceStatus = @InvoiceStatus,
                    AmountPaid = @AmountPaid,
                    BalanceDue = @BalanceDue,
                    UpdatedAt = @UpdatedAt,
                    Discount = @Discount
                WHERE InvoiceId = @InvoiceId";

            using var connection = context.CreateConnection();

            var affectedRows = await connection.ExecuteAsync(sql, invoice);

            if (affectedRows > 0)
            {
                logger.LogInformation("Successfully updated Invoice with ID: {Id}", id);
                return true;
            }

            logger.LogWarning("No rows updated for Invoice ID: {Id} (possibly not found or no changes)", id);
            return false;
        }

        #endregion

    }
}