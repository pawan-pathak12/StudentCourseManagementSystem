using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCourseManagement.Application.DTOs.FInancialModule.InvoiceLineItems;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.API.Controllers.FinancialModules
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceLineItemController : ControllerBase
    {
        private readonly IInvoiceLineItemService _lineItemService;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceLineItemController> _logger;

        public InvoiceLineItemController(IInvoiceLineItemService lineItemService, IMapper mapper, ILogger<InvoiceLineItemController> logger)
        {
            this._lineItemService = lineItemService;
            this._mapper = mapper;
            this._logger = logger;
        }

        #region HttpPost Endpoint
        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Create([FromBody] CreateInvoiceLineItemDto lineItemDto)
        {

            var invoiceLineItem = _mapper.Map<InvoiceLineItem>(lineItemDto);
            var (success, errorMessage, lineItemId) = await _lineItemService.CreateAsync(invoiceLineItem);

            if (!success)
            {
                _logger.LogWarning($"Failed to create InvoiceLineItem for Invoice Id {invoiceLineItem.InvoiceId}");
                return BadRequest($"Error Meaage : {errorMessage}");
            }

            return CreatedAtAction(nameof(GetById), new { id = lineItemId }, invoiceLineItem);
        }
        #endregion

        #region HttpGet Endpoint
        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetAll()
        {
            var invoiceLineItems = await _lineItemService.GetAllAsync();
            return Ok(invoiceLineItems);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var invoiceLineItem = await _lineItemService.GetByIdAsync(id);
            if (invoiceLineItem == null)
            {
                _logger.LogWarning($"InvoiceLineItem with Id {id} not found");
                return NotFound();
            }
            return Ok(invoiceLineItem);
        }
        #endregion

        #region HttpPut Endpoint
        [HttpPut("{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateInvoiceLineItemDto updateInvoiceLineItemDto)
        {

            var invoiceLineItem = _mapper.Map<InvoiceLineItem>(updateInvoiceLineItemDto);
            var isUpdated = await _lineItemService.UpdateAsync(id, invoiceLineItem);

            if (!isUpdated)
            {
                _logger.LogWarning($"Failed to update InvoiceLineItem with Id {id}");
                return BadRequest("Update failed");
            }

            return Ok("Update successful");
        }
        #endregion

        #region HttpDelete Endpoint
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var isDeleted = await _lineItemService.DeleteAsync(id);

            if (!isDeleted)
            {
                _logger.LogWarning($"Failed to delete InvoiceLineItem with Id {id}");
                return BadRequest("Delete failed");
            }

            return Ok("Delete successful");
        }
        #endregion
    }
}
