using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCourseManagement.Business.DTOs.FInancialModule.Refunds;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;

namespace StudentCourseManagement.API.Controllers.FinancialModules
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RefundController : ControllerBase
    {
        private readonly IRefundService _refundService;

        public RefundController(IRefundService refundService)
        {
            this._refundService = refundService;
        }

        #region HttpPost -process refund

        [HttpPost("process-refund")]
        public async Task<IActionResult> ProcessRefund([FromBody] ProcessRefundDto processRefund)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var (success, errorMessage) = await _refundService.ProcessRefundAsync(processRefund.PaymentId, processRefund.RefundReason);
            if (!success)
            {
                return BadRequest(errorMessage);
            }
            var refundInfo = await _refundService.GetRefundInfoAsync(processRefund.PaymentId);
            if (refundInfo == null)
            {
                return NotFound();
            }
            return Ok(refundInfo);
        }
        #endregion


        [HttpGet("refund-info")]
        public async Task<IActionResult> GetRefundInfoAsync(int paymentId)
        {
            var paymentInfo = await _refundService.GetRefundInfoAsync(paymentId);
            if (paymentInfo == null)
            {
                return NotFound();
            }
            return Ok(paymentInfo);
        }

        [HttpGet("check-refund-eligibility ")]
        public async Task<IActionResult> ValidateEligibility(int paymentId)
        {
            var (success, errorMessage) = await _refundService.ValidateEligibilityAsync(paymentId);
            if (!success)
            {
                return BadRequest(errorMessage);
            }
            return Ok(new { CanBeRefunded = true });
        }

    }
}
