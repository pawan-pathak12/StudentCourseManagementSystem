using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentCourseManagement.Application.DTOs.DTOs.FInancialModule.Payments;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.API.Controllers.FinancialModules
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, IMapper mapper, ILogger<PaymentController> logger)
        {
            this._paymentService = paymentService;
            this._mapper = mapper;
            this._logger = logger;
        }

        #region HttpPost Endpoint
        #region Manual 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDto paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payment = _mapper.Map<Payment>(paymentDto);
            var (success, errorMessage, paymentId) = await _paymentService.CreateAsync(payment);

            if (!success)
            {
                _logger.LogWarning($"Failed to create Payment for Student Id {payment.StudentId}");
                return BadRequest($"Error Message : {errorMessage}");
            }

            return CreatedAtAction(nameof(GetById), new { id = payment.InvoiceId }, payment);
        }
        #endregion

        #region Automated 
        [HttpPost("process-payment")]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentDto processPayment)
        {
            var (success, errorMessage) = await _paymentService.ProcessPaymentAsync(processPayment.InvoiceId, processPayment.PaymentMethodId, processPayment.PaidAmount);
            if (success)
            {
                var result = await _paymentService.GetPaymentDetailsByInvoiceIdAsync(processPayment.InvoiceId);
                return Ok(result);
            }
            return BadRequest(new { message = errorMessage });
        }
        #endregion

        #endregion

        #region HttpGet Endpoint
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _paymentService.GetAllAsync();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            if (payment == null)
            {
                _logger.LogWarning($"Payment with Id {id} not found");
                return NotFound();
            }
            return Ok(payment);
        }
        #endregion

        #region HttpPut Endpoint
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePaymentDto updatePaymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payment = _mapper.Map<Payment>(updatePaymentDto);
            var isUpdated = await _paymentService.UpdateAsync(id, payment);

            if (!isUpdated)
            {
                _logger.LogWarning($"Failed to update Payment with Id {id}");
                return BadRequest("Update failed");
            }
            return Ok("Update successful");
        }
        #endregion

        #region HttpDelete Endpoint
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var isDeleted = await _paymentService.DeleteAsync(id);

            if (!isDeleted)
            {
                _logger.LogWarning($"Failed to delete Payment with Id {id}");
                return BadRequest("Delete failed");
            }

            return Ok("Delete successful");
        }
        #endregion
    }
}
