using Microsoft.AspNetCore.Mvc;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;

namespace StudentCourseManagement.API.Controllers.FinancialModules
{
    [Route("api/[controller]")]
    [ApiController]
    public class LateFeeController : ControllerBase
    {
        private readonly ILateFeeService _lateFeeService;
        private readonly ILogger<LateFeeController> _logger;

        public LateFeeController(ILateFeeService lateFeeService, ILogger<LateFeeController> logger)
        {
            _lateFeeService = lateFeeService;
            _logger = logger;
        }

        [HttpGet("overdue-invoice/{invoiceId}")]
        public async Task<IActionResult> GetOverdueInvoiceAsync(int invoiceId)
        {
            var invoice = await _lateFeeService.GetOverDueInvoiceAsync(invoiceId);
            if (invoice == null)
            {
                _logger.LogWarning("Invoice {InvoiceId} not found or not overdue.", invoiceId);
                return NotFound();
            }

            return Ok(invoice);
        }

        [HttpPost("apply-late-fee/{invoiceId}")]
        public async Task<IActionResult> ApplyLateFeeAsync(int invoiceId)
        {
            var result = await _lateFeeService.ApplyLateFeeAsync(invoiceId);
            if (!result)
            {
                _logger.LogError("Failed to apply late fee to invoice {InvoiceId}", invoiceId);
                return BadRequest("Failed to apply late fee.");
            }

            return NoContent(); // or Ok(new { Success = true })
        }

        [HttpPost("apply-to-all-overdue")]
        public async Task<IActionResult> ApplyLateFeeToAllOverdueInvoicesAsync()
        {
            var (success, failed) = await _lateFeeService.ProcessAllOverDueAsync();
            _logger.LogInformation("Applied late fee to {SuccessCount} invoices, failed for {FailedCount}.", success, failed);

            return Ok(new
            {
                SuccessfulLateFee = success,
                FailedLateFee = failed
            });
        }
    }
}
