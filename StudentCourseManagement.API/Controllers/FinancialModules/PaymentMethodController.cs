using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentCourseManagement.Application.DTOs.FInancialModule.PaymentMethods;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.API.Controllers.FinancialModules
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentMethodController> _logger;

        public PaymentMethodController(IPaymentMethodService paymentMethodService, IMapper mapper, ILogger<PaymentMethodController> logger)
        {
            this._paymentMethodService = paymentMethodService;
            this._mapper = mapper;
            this._logger = logger;
        }

        #region HttpPost Endpoint
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentMethodDto createPaymentMethodDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentMethod = _mapper.Map<PaymentMethod>(createPaymentMethodDto);
            var isCreated = await _paymentMethodService.CreateAsync(paymentMethod);

            if (!isCreated)
            {
                _logger.LogWarning($"Failed to create Payment Method {paymentMethod}");
                return BadRequest("Failed to create Payment");
            }

            return CreatedAtAction(nameof(GetById), new { id = paymentMethod.PaymentMethodId }, paymentMethod);
        }
        #endregion

        #region HttpGet Endpoint
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var paymentMethods = await _paymentMethodService.GetAllAsync();
            return Ok(paymentMethods);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var paymentMethod = await _paymentMethodService.GetByIdAsync(id);
            if (paymentMethod == null)
            {
                _logger.LogWarning($"Payment Method with Id {id} not found");
                return NotFound();
            }
            return Ok(paymentMethod);
        }
        #endregion

        #region HttpPut Endpoint
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePaymentMethodDto updatePaymentMethodDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentMethod = _mapper.Map<PaymentMethod>(updatePaymentMethodDto);
            var isUpdated = await _paymentMethodService.UpdateAsync(id, paymentMethod);

            if (!isUpdated)
            {
                _logger.LogWarning($"Failed to update Payment Method with Id {id}");
                return BadRequest("Update failed");
            }
            return Ok("Update successful");
        }
        #endregion

        #region HttpDelete Endpoint
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var isDeleted = await _paymentMethodService.DeleteAsync(id);

            if (!isDeleted)
            {
                _logger.LogWarning($"Failed to delete Payment Method with Id {id}");
                return BadRequest("Delete failed");
            }

            return Ok("Delete successful");
        }
        #endregion

    }
}
