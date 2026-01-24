using Dapper;
using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Data.Database;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Data.Repositories.Dapper.FinancialModule
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly StudentSysDbContext _context;
        private readonly ILogger<PaymentRepository> logger;

        public PaymentRepository(StudentSysDbContext context, ILogger<PaymentRepository> logger)
        {
            this._context = context;
            this.logger = logger;
        }

        #region CURD Operations

        public async Task<int> AddAsync(Payment payment)
        {
            logger.LogInformation("Adding new Payment for InvoiceId: {InvoiceId}, StudentId: {StudentId}, Amount: {Amount}",
                payment.InvoiceId, payment.StudentId, payment.Amount);

            const string sql = @"
                INSERT INTO Payments (
                    StudentId, InvoiceId, Amount, PaymentDate, PaymentMethodId,RefundedPaymentId,RefundReason,
                    PaymentStatus, ReferenceNumber, Notes, ProcessedBy, CreatedDate,IsActive,RefundDate
                )
                VALUES (
                    @StudentId, @InvoiceId, @Amount, @PaymentDate, @PaymentMethodId,@RefundedPaymentId,@RefundReason, 
                    @PaymentStatus, @ReferenceNumber, @Notes, @ProcessedBy, @CreatedDate,@IsActive,@RefundDate
                );
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = _context.CreateConnection();

            var newId = await connection.ExecuteScalarAsync<int>(sql, payment);

            logger.LogInformation("Successfully added Payment with ID: {Id}, ReferenceNumber: {ReferenceNumber}",
                newId, payment.ReferenceNumber);

            return newId;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = " Update Payments set IsActive =0 WHERE PaymentId = @Id and IsActive=1";

            using var connection = _context.CreateConnection();

            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });

            if (affectedRows > 0)
            {
                logger.LogInformation("Successfully softly  deleted Payment with ID: {Id}", id);
                return true;
            }

            logger.LogWarning("Payment with ID: {Id} not found for deletion", id);
            return false;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {

            const string sql = "SELECT * FROM Payments where IsActive=1  ORDER BY PaymentDate DESC ";

            using var connection = _context.CreateConnection();

            var payments = await connection.QueryAsync<Payment>(sql);

            logger.LogInformation("Successfully retrieved {Count} Payments", payments.Count());

            return payments;
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {

            const string sql = "SELECT * FROM Payments WHERE PaymentId = @Id and IsActive=1";

            using var connection = _context.CreateConnection();

            var payment = await connection.QuerySingleOrDefaultAsync<Payment>(sql, new { Id = id });

            if (payment == null)
            {
                logger.LogWarning("Payment with ID: {Id} not found", id);
            }
            else
            {
                logger.LogInformation("Successfully retrieved Payment with ID: {Id}, ReferenceNumber: {ReferenceNumber}",
                    id, payment.ReferenceNumber);
            }

            return payment;
        }

        public async Task<bool> UpdateAsync(int id, Payment payment)
        {
            if (id != payment.PaymentId)
            {
                logger.LogWarning("Update failed: ID mismatch. Provided ID: {ProvidedId}, Entity ID: {EntityId}",
                    id, payment.PaymentId);
                return false;
            }

            const string sql = @"
                UPDATE Payments 
                SET StudentId = @StudentId,
                    InvoiceId = @InvoiceId,
                    Amount = @Amount,
                    PaymentDate = @PaymentDate,
                    PaymentMethodId = @PaymentMethodId,
                    PaymentStatus = @PaymentStatus,
                    ReferenceNumber = @ReferenceNumber,
                    IsActive=@IsActive,
                    Notes = @Notes,
                    ProcessedBy = @ProcessedBy
                WHERE PaymentId = @PaymentId and IsActive=1";

            using var connection = _context.CreateConnection();

            var affectedRows = await connection.ExecuteAsync(sql, payment);

            if (affectedRows > 0)
            {
                logger.LogInformation("Successfully updated Payment with ID: {Id}", id);
                return true;
            }

            logger.LogWarning("Update failed for Payment ID: {Id} ", id);
            return false;
        }

        #endregion

        #region Phase 4  : 
        public async Task<Payment?> GetByInvoiceIdAsync(int invoiceId)
        {
            const string sql = @"select p.*
                            from Payments p
                            inner join Invoices i on p.InvoiceId = i.InvoiceId
                            where i.InvoiceId =@InvoiceId
                            and i.IsActive =1 
                            and p.IsActive=1";
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Payment>(sql, new { InvoiceId = invoiceId });
            if (result != null)
            {
                logger.LogWarning($"Feteched Payment record with invoice Id {invoiceId}");
            }
            return result;

        }

        #endregion

        #region Phase 5 : refund payment 

        public async Task<Invoice?> GetInvoiceByPaymentIdAsync(int paymentId)
        {
            const string sql = @"select  i.*
                            from Payments p
                            inner join Invoices i on p.InvoiceId =i.InvoiceId
                            where p.PaymentId = @PaymentId";
            using var connection = _context.CreateConnection();
            var invoice = await connection.QuerySingleOrDefaultAsync<Invoice>(sql, new { PaymentId = paymentId });
            return invoice;
        }

        public async Task<bool> IsRefundedAsync(int paymentId)
        {
            const string sql = @"select 
                            case when Exists (select 1 from Payments 
                            where RefundedPaymentId=@PaymentId)
                            then 1 else 0 end ";

            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteScalarAsync<bool>(sql, new { PaymentId = paymentId });
            return result;
        }
        public async Task<int> GetEnrollmentIdFromPaymentIdAsync(int paymentId)
        {
            const string sql = @"select e.EnrollmentId
                            from Payments p
                            inner join Invoices i ON p.InvoiceId = i.InvoiceId
                            inner join FeeAssessments f ON i.FeeAssessmentId = f.FeeAssessmentId
                            inner join Enrollments e ON f.EnrollmentId = e.EnrollmentId
                            where p.PaymentId = @PaymentId;";
            using var connection = _context.CreateConnection();
            var enrollmentId = await connection.ExecuteScalarAsync<int>(sql, new { PaymentId = paymentId });
            return enrollmentId;
        }

        public async Task<Payment?> GetRefundPaymentDataByPaymentId(int paymentId)
        {
            const string sql = @"select * from Payments
                                where RefundedPaymentId =@PaymentId;";
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<Payment>(sql, new { PaymentId = paymentId });
            return result;
        }

        #endregion

    }
}