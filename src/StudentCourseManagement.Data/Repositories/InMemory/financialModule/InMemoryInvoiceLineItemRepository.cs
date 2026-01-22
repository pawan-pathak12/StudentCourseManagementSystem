using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

public class InMemoryInvoiceLineItemRepository : IInvoiceLineItemRepository
{
    private readonly List<InvoiceLineItem> _lineItems;
    private readonly IMapper _mapper;

    public InMemoryInvoiceLineItemRepository(IMapper mapper)
    {
        _lineItems = new List<InvoiceLineItem>();
        this._mapper = mapper;
    }

    #region CRUD Operations
    public Task<int> AddAsync(InvoiceLineItem lineItem)
    {
        _lineItems.Add(lineItem);
        lineItem.InvoiceLineItemId++;
        return Task.FromResult(lineItem.InvoiceLineItemId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var lineItem = await GetByIdAsync(id);
        if (lineItem == null)
        {
            return false;
        }
        lineItem.IsActive = false;
        return true;
    }

    public Task<IEnumerable<InvoiceLineItem>> GetAllAsync()
    {
        var activeItems = _lineItems.Where(x => x.IsActive).AsEnumerable();
        return Task.FromResult(activeItems);
    }

    public Task<InvoiceLineItem?> GetByIdAsync(int id)
    {
        var lineItem = _lineItems.FirstOrDefault(x => x.InvoiceLineItemId == id && x.IsActive);
        return Task.FromResult(lineItem);
    }

    public Task<bool> UpdateAsync(int id, InvoiceLineItem lineItem)
    {
        var existing = _lineItems.FirstOrDefault(x => x.InvoiceLineItemId == id && x.IsActive);
        if (existing == null)
        {
            return Task.FromResult(false);
        }
        _mapper.Map(lineItem, existing);
        return Task.FromResult(true);
    }
    #endregion
}