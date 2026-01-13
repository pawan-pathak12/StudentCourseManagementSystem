// Application Layer - Service
using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(IPaymentRepository paymentRepository, ILogger<PaymentService> logger)
    {
        _paymentRepository = paymentRepository;
        _logger = logger;
    }

    #region CRUD Operations
    public async Task<bool> CreateAsync(Payment payment)
    {
        await _paymentRepository.AddAsync(payment);
        return true;
    }

    public async Task<bool> DeleteAsync(int paymentId)
    {
        var payment = await GetByIdAsync(paymentId);
        if (payment == null)
        {
            _logger.LogWarning($"Payment with Id {paymentId} not found");
            return false;
        }
        return await _paymentRepository.DeleteAsync(paymentId);
    }

    public async Task<IEnumerable<Payment>> GetAllAsync()
    {
        return await _paymentRepository.GetAllAsync();
    }

    public async Task<Payment?> GetByIdAsync(int paymentId)
    {
        return await _paymentRepository.GetByIdAsync(paymentId);
    }

    public async Task<bool> UpdateAsync(int paymentId, Payment payment)
    {
        if (paymentId != payment.PaymentId)
        {
            _logger.LogWarning("Id mismatched");
            return false;
        }
        return await _paymentRepository.UpdateAsync(paymentId, payment);
    }
    #endregion
}

