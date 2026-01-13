// Application Layer - Service
using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

public class InvoiceLineItemService : IInvoiceLineItemService
{
    private readonly IInvoiceLineItemRepository _lineItemRepository;
    private readonly ILogger<InvoiceLineItemService> _logger;

    public InvoiceLineItemService(IInvoiceLineItemRepository lineItemRepository, ILogger<InvoiceLineItemService> logger)
    {
        _lineItemRepository = lineItemRepository;
        _logger = logger;
    }

    #region CRUD Operations
    public async Task<bool> CreateAsync(InvoiceLineItem lineItem)
    {
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
        return await _lineItemRepository.UpdateAsync(lineItemId, lineItem);
    }
    #endregion
}

