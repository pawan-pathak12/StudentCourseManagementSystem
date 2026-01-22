using Dapper;
using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Data.Database;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Data.Repositories.Dapper.FinancialModule
{
    public class InvoiceLineItemRepository : IInvoiceLineItemRepository
    {
        private readonly StudentSysDbContext context;
        private readonly ILogger<InvoiceLineItemRepository> logger;

        public InvoiceLineItemRepository(StudentSysDbContext context, ILogger<InvoiceLineItemRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<int> AddAsync(InvoiceLineItem invoiceLineItem)
        {
            logger.LogInformation("Adding new InvoiceLineItem for InvoiceId: {InvoiceId}, Description: {Description}, Amount: {Amount}",
                invoiceLineItem.InvoiceId, invoiceLineItem.Description, invoiceLineItem.Amount);

            invoiceLineItem.CreatedAt = DateTimeOffset.UtcNow;
            const string sql = @"
                INSERT INTO InvoiceLineItems (
                    FeeTemplateId, CourseId, InvoiceId, Description, 
                    Quantity, UnitPrice, Amount, CreatedAt,IsActive
                )
                VALUES (
                    @FeeTemplateId, @CourseId, @InvoiceId, @Description,
                    @Quantity, @UnitPrice, @Amount, @CreatedAt,@IsActive
                );
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = context.CreateConnection();

            var newId = await connection.QuerySingleAsync<int>(sql, invoiceLineItem);

            logger.LogInformation("Successfully added InvoiceLineItem with ID: {Id} for InvoiceId: {InvoiceId}",
                newId, invoiceLineItem.InvoiceId);

            return newId;
        }

        public async Task<bool> DeleteAsync(int id)
        {

            const string sql = "Update InvoiceLineItems set IsActive =0 WHERE InvoiceLineItemId = @Id";

            using var connection = context.CreateConnection();

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });

            if (affectedRows > 0)
            {
                logger.LogInformation("Successfully deleted InvoiceLineItem with ID: {Id}", id);
                return true;
            }

            logger.LogWarning("InvoiceLineItem with ID: {Id} not found for deletion", id);
            return false;
        }

        public async Task<IEnumerable<InvoiceLineItem>> GetAllAsync()
        {
            logger.LogInformation("Retrieving all InvoiceLineItems");

            const string sql = "SELECT * FROM InvoiceLineItems where IsActive=1";

            using var connection = context.CreateConnection();

            var lineItems = await connection.QueryAsync<InvoiceLineItem>(sql);

            logger.LogInformation("Successfully retrieved {Count} InvoiceLineItems", lineItems.Count());

            return lineItems;
        }

        public async Task<InvoiceLineItem?> GetByIdAsync(int id)
        {

            const string sql = "SELECT * FROM InvoiceLineItems WHERE InvoiceLineItemId = @Id and IsActive=1";

            using var connection = context.CreateConnection();

            var lineItem = await connection.QuerySingleOrDefaultAsync<InvoiceLineItem>(sql, new { Id = id });

            if (lineItem == null)
            {
                logger.LogWarning("InvoiceLineItem with ID: {Id} not found", id);
            }
            else
            {
                logger.LogInformation("Successfully retrieved InvoiceLineItem with ID: {Id}", id);
            }

            return lineItem;
        }

        public async Task<bool> UpdateAsync(int id, InvoiceLineItem invoiceLineItem)
        {
            if (id != invoiceLineItem.InvoiceLineItemId)
            {
                logger.LogWarning("Update failed: ID mismatch. Provided ID: {ProvidedId}, Entity ID: {EntityId}",
                    id, invoiceLineItem.InvoiceLineItemId);
                return false;
            }

            const string sql = @"
                UPDATE InvoiceLineItems 
                SET FeeTemplateId = @FeeTemplateId,
                    CourseId = @CourseId,
                    InvoiceId = @InvoiceId,
                    Description = @Description,
                    Quantity = @Quantity,
                    UnitPrice = @UnitPrice,
                    Amount = @Amount
                WHERE InvoiceLineItemId = @InvoiceLineItemId";

            using var connection = context.CreateConnection();

            var affectedRows = await connection.ExecuteAsync(sql, invoiceLineItem);

            if (affectedRows > 0)
            {
                logger.LogInformation("Successfully updated InvoiceLineItem with ID: {Id}", id);
                return true;
            }

            logger.LogWarning("Update Failed for InvoiceLineItem ID: {Id}", id);
            return false;
        }
    }
}