
using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

public class InMemoryPaymentMethodRepository : IPaymentMethodRepository
{
    private readonly List<PaymentMethod> _paymentMethods;
    private readonly IMapper _mapper;

    public InMemoryPaymentMethodRepository(IMapper mapper)
    {
        _paymentMethods = new List<PaymentMethod>();
        this._mapper = mapper;
    }

    #region CRUD Operations
    public Task<int> AddAsync(PaymentMethod paymentMethod)
    {
        _paymentMethods.Add(paymentMethod);
        paymentMethod.PaymentMethodId = _paymentMethods.Count;
        return Task.FromResult(paymentMethod.PaymentMethodId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var method = await GetByIdAsync(id);
        if (method == null)
        {
            return false;
        }
        method.IsActive = false;
        return true;
    }

    public Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
        var activeMethods = _paymentMethods.Where(x => x.IsActive).AsEnumerable();
        return Task.FromResult(activeMethods);
    }

    public Task<PaymentMethod?> GetByIdAsync(int id)
    {
        var method = _paymentMethods.FirstOrDefault(x => x.PaymentMethodId == id && x.IsActive);
        return Task.FromResult(method);
    }

    public Task<bool> UpdateAsync(int id, PaymentMethod paymentMethod)
    {
        var existing = _paymentMethods.FirstOrDefault(x => x.PaymentMethodId == id && x.IsActive);
        if (existing == null)
        {
            return Task.FromResult(false);
        }
        _mapper.Map(paymentMethod, existing);
        return Task.FromResult(true);
    }
    #endregion

    public Task<PaymentMethod?> GetByNameAsync(string paymentMethodName)
    {
        throw new NotImplementedException();
    }


}