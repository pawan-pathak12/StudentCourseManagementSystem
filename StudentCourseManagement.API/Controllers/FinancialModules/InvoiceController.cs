using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentCourseManagement.Application.DTOs.DTOs.FInancialModule.Invoices;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.API.Controllers.FinancialModules
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(IInvoiceService invoiceService, IMapper mapper, ILogger<InvoiceController> logger)
        {
            this._invoiceService = invoiceService;
            this._mapper = mapper;
            this._logger = logger;
        }

        #region HttpPost Endpoint
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInvoiceDto createInvoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var invoice = _mapper.Map<Invoice>(createInvoice);
            var (success, errorMessage, invoiceId) = await _invoiceService.CreateAsync(invoice);

            if (!success)
            {
                _logger.LogWarning($"Failed to create Invoice for feeAssessmentId {invoice.FeeAssessmentId}");
                return BadRequest($"Error Meaage : {errorMessage}");
            }

            return CreatedAtAction(nameof(GetById), new { id = invoiceId }, createInvoice);
        }
        #endregion

        #region HttpGet Endpoint
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var invoices = await _invoiceService.GetAllAsync();
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            if (invoice == null)
            {
                _logger.LogWarning($"Invoice with Id {id} not found");
                return NotFound();
            }
            return Ok(invoice);
        }
        #endregion

        #region HttpPut Endpoint
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateInvoiceDto updateInvoiceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var invoice = _mapper.Map<Invoice>(updateInvoiceDto);
            var isUpdated = await _invoiceService.UpdateAsync(id, invoice);

            if (!isUpdated)
            {
                _logger.LogWarning($"Failed to update Invoice with Id {id}");
                return BadRequest("Update failed");
            }

            return Ok("Update successful");
        }
        #endregion

        #region HttpDelete Endpoint
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var isDeleted = await _invoiceService.DeleteAsync(id);

            if (!isDeleted)
            {
                _logger.LogWarning($"Failed to delete Invoice with Id {id}");
                return BadRequest("Delete failed");
            }

            return Ok("Delete successful");
        }
        #endregion

    }
}
