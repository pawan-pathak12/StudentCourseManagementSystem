
using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

public class InvoiceLineItemService : IInvoiceLineItemService
{
    private readonly IInvoiceLineItemRepository _lineItemRepository;
    private readonly ILogger<InvoiceLineItemService> _logger;
    private readonly ICourseRepository _courseRepository;
    private readonly IFeeTemplateRepository _feeTemplateRepository;
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceLineItemService(IInvoiceLineItemRepository lineItemRepository, ILogger<InvoiceLineItemService> logger, ICourseRepository courseRepository,
        IFeeTemplateRepository feeTemplateRepository, IInvoiceRepository invoiceRepository)
    {
        _lineItemRepository = lineItemRepository;
        _logger = logger;
        this._courseRepository = courseRepository;
        this._feeTemplateRepository = feeTemplateRepository;
        this._invoiceRepository = invoiceRepository;
    }

    #region CRUD Operations
    public async Task<bool> CreateAsync(InvoiceLineItem lineItem)
    {
        var course = await _courseRepository.GetByIdAsync(lineItem.CourseId);
        if (course == null)
        {
            _logger.LogWarning("Failed to create InvoiceLineItem: Course with Id {CourseId} not found", lineItem.CourseId);
            return false;
        }

        var feeTemplate = await _feeTemplateRepository.GetByIdAsync(lineItem.FeeTemplateId);
        if (feeTemplate == null)
        {
            _logger.LogWarning("Failed to create InvoiceLineItem: FeeTemplate with Id {FeeTemplateId} not found", lineItem.FeeTemplateId);
            return false;
        }

        var invoice = await _invoiceRepository.GetByIdAsync(lineItem.InvoiceId);
        if (invoice == null)
        {
            _logger.LogWarning("Failed to create InvoiceLineItem: Invoice with Id {InvoiceId} not found", lineItem.InvoiceId);
            return false;
        }

        await _lineItemRepository.AddAsync(lineItem);
        return true;
    }

    public async Task<bool> DeleteAsync(int lineItemId)
    {
        var lineItem = await GetByIdAsync(lineItemId);
        if (lineItem == null)
        {
            _logger.LogWarning($"InvoiceLineItem with Id {lineItemId} not found");
            return false;
        }
        return await _lineItemRepository.DeleteAsync(lineItemId);
    }

    public async Task<IEnumerable<InvoiceLineItem>> GetAllAsync()
    {
        return await _lineItemRepository.GetAllAsync();
    }

    public async Task<InvoiceLineItem?> GetByIdAsync(int lineItemId)
    {
        return await _lineItemRepository.GetByIdAsync(lineItemId);
    }

    public async Task<bool> UpdateAsync(int lineItemId, InvoiceLineItem lineItem)
    {
        if (lineItemId != lineItem.InvoiceLineItemId)
        {
            _logger.LogWarning("Id mismatched");
            return false;
        }
        var lineItemData = await _lineItemRepository.GetByIdAsync(lineItemId);
        if (lineItemData == null)
        {
            _logger.LogWarning($"Failed to Update InvoiceLineItem: InvoiceLineItem with Id {lineItemId} not found");
            return false;
        }
        await _lineItemRepository.UpdateAsync(lineItemId, lineItem);
        return true;
    }
    #endregion
}

