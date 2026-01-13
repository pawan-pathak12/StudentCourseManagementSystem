// Application Layer - Service
using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

public class PaymentMethodService : IPaymentMethodService
{
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly ILogger<PaymentMethodService> _logger;

    public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository, ILogger<PaymentMethodService> logger)
    {
        _paymentMethodRepository = paymentMethodRepository;
        _logger = logger;
    }

    #region CRUD Operations
    public async Task<bool> CreateAsync(PaymentMethod paymentMethod)
    {
        await _paymentMethodRepository.AddAsync(paymentMethod);
        return true;
    }

    public async Task<bool> DeleteAsync(int paymentMethodId)
    {
        var method = await GetByIdAsync(paymentMethodId);
        if (method == null)
        {
            _logger.LogWarning($"PaymentMethod with Id {paymentMethodId} not found");
            return false;
        }
        return await _paymentMethodRepository.DeleteAsync(paymentMethodId);
    }

    public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
        return await _paymentMethodRepository.GetAllAsync();
    }

    public async Task<PaymentMethod?> GetByIdAsync(int paymentMethodId)
    {
        return await _paymentMethodRepository.GetByIdAsync(paymentMethodId);
    }

    public async Task<bool> UpdateAsync(int paymentMethodId, PaymentMethod paymentMethod)
    {
        if (paymentMethodId != paymentMethod.PaymentMethodId)
        {
            _logger.LogWarning("Id mismatched");
            return false;
        }
        return await _paymentMethodRepository.UpdateAsync(paymentMethodId, paymentMethod);
    }
    #endregion
}
