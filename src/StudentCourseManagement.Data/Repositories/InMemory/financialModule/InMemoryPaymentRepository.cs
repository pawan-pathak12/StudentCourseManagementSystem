using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Data.Repositories.InMemory.financialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

public class InMemoryPaymentRepository : IPaymentRepository
{
    private readonly List<Payment> _payments;
    private readonly IMapper _mapper;
    private readonly InMemorryInvoiceRepository _invoiceRepository;
    private readonly IFeeAssessmentRepository _feeAssessmentRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;

    public InMemoryPaymentRepository(IMapper mapper, InMemorryInvoiceRepository invoiceRepository, IFeeAssessmentRepository feeAssessmentRepository, IEnrollmentRepository enrollmentRepositorys)
    {
        _payments = new List<Payment>();
        this._mapper = mapper;
        this._invoiceRepository = invoiceRepository;
        this._feeAssessmentRepository = feeAssessmentRepository;
        this._enrollmentRepository = _enrollmentRepository;
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
    public async Task<Invoice?> GetInvoiceByPaymentIdAsync(int paymentId)
    {
        var payment = _payments.Find(x => x.PaymentId == paymentId);
        if (payment == null) return null;

        return await _invoiceRepository.GetByIdAsync(payment.InvoiceId);
    }

    public Task<bool> IsRefundedAsync(int paymentId)
    {
        var isRefundable = _payments.Exists(x => x.RefundedPaymentId == paymentId && x.IsActive);
        return Task.FromResult(isRefundable);
    }

    public async Task<int> GetEnrollmentIdFromPaymentIdAsync(int paymentId)
    {
        var payment = _payments.Find(x => x.PaymentId == paymentId);
        if (payment == null) return 0;

        var invoice = await _invoiceRepository.GetByIdAsync(payment.InvoiceId);
        if (invoice == null) return 0;

        var feeAssessment = await _feeAssessmentRepository.GetByIdAsync(invoice.FeeAssessmentId);
        if (feeAssessment == null) return 0;

        var enrollment = await _enrollmentRepository.GetByIdAsync(feeAssessment.EnrollmentId);
        if (enrollment == null)
        {
            return 0;
        }
        return enrollment.EnrollmentId;
    }

    public Task<Payment?> GetRefundPaymentDataByPaymentId(int paymentId)
    {
        var payment = _payments.Find(x => x.RefundedPaymentId == paymentId);
        return Task.FromResult(payment);
    }
    #endregion

}