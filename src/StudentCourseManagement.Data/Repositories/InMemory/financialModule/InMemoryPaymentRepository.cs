using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

public class InMemoryPaymentRepository : IPaymentRepository
{
    private readonly List<Payment> _payments;
    private readonly IMapper _mapper;

    public InMemoryPaymentRepository(IMapper mapper)
    {
        _payments = new List<Payment>();
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
        throw new NotImplementedException();
    }

    public Task<bool> IsRefundedAsync(int paymentId)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetEnrollmentIdFromPaymentIdAsync(int paymentId)
    {
        throw new NotImplementedException();
    }

    public Task<Payment?> GetRefundPaymentDataByPaymentId(int paymentId)
    {
        throw new NotImplementedException();
    }
    #endregion
}