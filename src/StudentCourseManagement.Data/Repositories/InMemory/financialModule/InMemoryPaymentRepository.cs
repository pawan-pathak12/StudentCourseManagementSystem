using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Data.Repositories.InMemory;
using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;

public class InMemoryPaymentRepository : IPaymentRepository
{
    private readonly List<Payment> _payments;
    private readonly List<Invoice> _invoices;
    private readonly List<FeeAssessment> _feeAssessments;
    private readonly List<Enrollment> _enrollments;
    private readonly IMapper _mapper;
    private readonly InMemoryDbContext _db;

    public InMemoryPaymentRepository(IMapper mapper, InMemoryDbContext db)
    {
        this._db = db;
        this._enrollments = _db.Enrollments;
        this._feeAssessments = _db.FeeAssessments;
        this._invoices = _db.Invoices;
        _payments = _db.Payments;
        this._mapper = mapper;


    }

    #region CRUD Operations
    public Task<int> AddAsync(Payment payment)
    {
        _payments.Add(payment);
        payment.PaymentId = _payments.Count;
        return Task.FromResult(payment.PaymentId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var payment = await GetByIdAsync(id);
        if (payment == null)
        {
            return false;
        }
        payment.IsActive = false;
        return true;
    }

    public Task<IEnumerable<Payment>> GetAllAsync()
    {
        var activePayments = _payments.Where(x => x.IsActive).AsEnumerable();
        return Task.FromResult(activePayments);
    }

    public Task<Payment?> GetByIdAsync(int id)
    {
        var payment = _payments.FirstOrDefault(x => x.PaymentId == id && x.IsActive);
        return Task.FromResult(payment);
    }



    public Task<bool> UpdateAsync(int id, Payment payment)
    {
        var existing = _payments.FirstOrDefault(x => x.PaymentId == id && x.IsActive);
        if (existing == null)
        {
            return Task.FromResult(false);
        }
        _mapper.Map(payment, existing);
        return Task.FromResult(true);
    }


    #endregion

    #region Phase 4 : 
    public Task<Payment?> GetByInvoiceIdAsync(int invoiceId)
    {
        var payment = _payments.Find(x => x.InvoiceId == invoiceId && x.IsActive);
        return Task.FromResult(payment);
    }

    #endregion

    #region Phase 5
    public Task<Invoice?> GetInvoiceByPaymentIdAsync(int paymentId)
    {
        var payment = _payments.Find(x => x.PaymentId == paymentId);
        if (payment == null) return null;

        var invoice = _invoices.Find(x => x.InvoiceId == payment.InvoiceId);
        return Task.FromResult(invoice);
    }

    #endregion

    #region Phase 6 

    public Task<bool> IsRefundedAsync(int paymentId)
    {
        var isRefundable = _payments.Exists(x => x.RefundedPaymentId == paymentId && x.IsActive);
        return Task.FromResult(isRefundable);
    }

    public Task<int> GetEnrollmentIdFromPaymentIdAsync(int paymentId)
    {
        var payment = _payments.Find(x => x.PaymentId == paymentId);
        if (payment == null) return Task.FromResult(0);

        var invoice = _invoices.Find(x => x.InvoiceId == payment.InvoiceId);
        if (invoice == null) return Task.FromResult(0);


        var feeAssessment = _feeAssessments.Find(x => x.FeeAssessmentId == invoice.FeeAssessmentId);
        if (feeAssessment == null) return Task.FromResult(0);

        var enrollment = _enrollments.Find(x => x.EnrollmentId == feeAssessment.EnrollmentId);
        if (enrollment == null)
        {
            return Task.FromResult(0);
        }
        return Task.FromResult(enrollment.EnrollmentId);
    }

    public Task<Payment?> GetRefundPaymentDataByPaymentId(int paymentId)
    {
        var payment = _payments.Find(x => x.RefundedPaymentId == paymentId);
        return Task.FromResult(payment);
    }

    #endregion


}