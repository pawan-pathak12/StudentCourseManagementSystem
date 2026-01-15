// Application Layer - Service
using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILogger<PaymentService> _logger;
    private readonly IStudentRepository _studentRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IInvoiceRepository _invoiceRepository;

    public PaymentService(IPaymentRepository paymentRepository, ILogger<PaymentService> logger, IStudentRepository studentRepository,
        IPaymentMethodRepository paymentMethodRepository, IInvoiceRepository invoiceRepository)
    {
        _paymentRepository = paymentRepository;
        _logger = logger;
        this._studentRepository = studentRepository;
        this._paymentMethodRepository = paymentMethodRepository;
        this._invoiceRepository = invoiceRepository;
    }

    #region CRUD Operations
    public async Task<bool> CreateAsync(Payment payment)
    {
        var student = await _studentRepository.GetByIdAsync(payment.StudentId);
        if (student == null)
        {
            _logger.LogWarning("Payment creation failed: Student with Id {StudentId} not found", payment.StudentId);
            return false;
        }

        var paymentMethod = await _paymentMethodRepository.GetByIdAsync(payment.PaymentMethodId);
        if (paymentMethod == null)
        {
            _logger.LogWarning("Payment creation failed: PaymentMethod with Id {PaymentMethodId} not found", payment.PaymentMethodId);
            return false;
        }

        var invoice = await _invoiceRepository.GetByIdAsync(payment.InvoiceId);
        if (invoice == null)
        {
            _logger.LogWarning("Payment creation failed: Invoice with Id {InvoiceId} not found", payment.InvoiceId);
            return false;
        }

        await _paymentRepository.AddAsync(payment);

        _logger.LogInformation("Payment created successfully for StudentId {StudentId}, InvoiceId {InvoiceId}, PaymentMethodId {PaymentMethodId}",
            payment.StudentId, payment.InvoiceId, payment.PaymentMethodId);
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
        var existingPayment = await _paymentRepository.GetByIdAsync(paymentId);
        if (existingPayment == null)
        {
            _logger.LogWarning($"Payment update failed: Payment with Id {paymentId} not found");
            return false;
        }
        return await _paymentRepository.UpdateAsync(paymentId, payment);
    }
    #endregion
}

