using Microsoft.Extensions.Logging;
using StudentCourseManagement.Application.DTOs.DTOs.FInancialModule.Payments;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using System.Transactions;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILogger<PaymentService> _logger;
    private readonly IStudentRepository _studentRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IFeeAssessmentRepository _feeAssessmentRepository;

    public PaymentService(IPaymentRepository paymentRepository, ILogger<PaymentService> logger, IStudentRepository studentRepository,
        IPaymentMethodRepository paymentMethodRepository, IInvoiceRepository invoiceRepository, IFeeAssessmentRepository feeAssessmentRepository)
    {
        _paymentRepository = paymentRepository;
        _logger = logger;
        this._studentRepository = studentRepository;
        this._paymentMethodRepository = paymentMethodRepository;
        this._invoiceRepository = invoiceRepository;
        this._feeAssessmentRepository = feeAssessmentRepository;
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

    #region Automated Payment Processing 
    public async Task<(bool success, string ErrorMessage)> ProcessPaymentAsync(int invoiceId, int paymentMethodId, decimal paidAmount)
    {
        #region Validation

        //1. invoice must exists 
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice == null)
        {
            _logger.LogWarning("Payment processing failed: Invoice with Id {InvoiceId} not found.", invoiceId);
            return (false, $"Invoice {invoiceId} not found.");
        }

        //2.invoice status must be payable 
        if (invoice.InvoiceStatus != InvoiceStatus.Issued && invoice.InvoiceStatus != InvoiceStatus.PartiallyPaid)
        {
            _logger.LogWarning("Payment processing failed: Invoice {InvoiceId} has non-payable status {Status}.", invoiceId, invoice.InvoiceStatus);
            return (false, $"Invoice {invoiceId} is not payable.");
        }

        //3. paymentMethod Must exists 
        var paymentMethodData = await _paymentMethodRepository.GetByIdAsync(paymentMethodId);
        if (paymentMethodData == null)
        {
            _logger.LogWarning("Payment processing failed: Payment method with Id {PaymentMethodId} not found.", paymentMethodId);
            return (false, $"Payment method {paymentMethodId} not found.");
        }

        //4. Payment method must be active 
        if (!paymentMethodData.IsActive)
        {
            _logger.LogWarning("Payment processing failed: Payment method {PaymentMethodId} is inactive.", paymentMethodId);
            return (false, $"Payment method {paymentMethodId} is inactive.");
        }

        //5.Enter amount must be positive
        if (paidAmount <= 0)
        {
            _logger.LogWarning("Payment processing failed: Invalid paid amount {PaidAmount} for Invoice {InvoiceId}.", paidAmount, invoiceId);
            return (false, $"Paid amount must be positive.");
        }

        //6. Amount to pay should be lower then balance due 
        if (paidAmount > invoice.BalanceDue)
        {
            _logger.LogWarning("Payment processing failed: Paid amount {PaidAmount} exceeds balance due {BalanceDue} for Invoice {InvoiceId}.", paidAmount, invoice.BalanceDue, invoiceId);
            return (false, $"Paid amount exceeds balance due.");

        }

        //7.

        #endregion

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var payment = new Payment
        {
            InvoiceId = invoiceId,
            PaymentMethodId = paymentMethodId,
            Amount = paidAmount,
            PaymentDate = DateTimeOffset.UtcNow,
            PaymentStatus = PaymentStatus.Completed,
            IsActive = true,
            CreatedDate = DateTimeOffset.UtcNow,
        };

        var paymentId = await _paymentRepository.AddAsync(payment);

        //update invoice 
        invoice.AmountPaid = invoice.AmountPaid + payment.Amount;
        invoice.BalanceDue = invoice.TotalAmount - invoice.AmountPaid;

        if (invoice.BalanceDue <= 0)
        {
            invoice.InvoiceStatus = InvoiceStatus.Paid;
        }
        else
        {
            invoice.InvoiceStatus = InvoiceStatus.PartiallyPaid;
        }

        await _invoiceRepository.UpdateAsync(invoice.InvoiceId, invoice);

        //Update FeeAssessment only if fully paid 
        if (invoice.InvoiceStatus == InvoiceStatus.Paid)
        {
            var feeAssessment = await _feeAssessmentRepository.GetByInvoiceIdAsync(invoiceId);
            if (feeAssessment != null)
            {
                feeAssessment.PaidDate = DateTimeOffset.UtcNow;
                feeAssessment.FeeAssessmentStatus = AssessmentStatus.Paid;
                await _feeAssessmentRepository.UpdateAsync(feeAssessment.FeeAssessmentId, feeAssessment);
            }
            else
            {
                _logger.LogWarning("FeeAssessment not found for Invoice {InvoiceId}. Skipping FeeAssessment update.", invoiceId);
            }
            scope.Complete();
        }
        return (true, null);
    }
    public async Task<PaymentResultDto> GetPaymentDetailsByInvoiceIdAsync(int invoiceId)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice == null)
        {
            return null;
        }
        var payment = await _paymentRepository.GetByInvoiceIdAsync(invoiceId);
        if (payment == null)
        {
            return null;
        }


        return new PaymentResultDto
        {
            InvoiceId = invoice.InvoiceId,
            TotalAmount = invoice.TotalAmount,
            BalanceDue = invoice.BalanceDue,
            FeeAssessmentId = invoice.FeeAssessmentId,
            PaidAmount = invoice.AmountPaid,
            InvoiceStatus = invoice.InvoiceStatus,
            PaymentId = payment.PaymentId,
            PaymentMethodId = payment.PaymentMethodId
        };
    }
    #endregion
}

