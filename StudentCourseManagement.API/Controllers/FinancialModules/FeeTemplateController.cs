using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentCourseManagement.Application.DTOs.DTOs.FInancialModule.feeTemplates;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.API.Controllers.FinancialModules
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeeTemplateController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<FeeTemplateController> _logger;
        private readonly IFeeTemplateService _feeTemplateService;

        public FeeTemplateController(IMapper mapper, ILogger<FeeTemplateController> logger, IFeeTemplateService feeTemplateService)
        {
            this._mapper = mapper;
            this._logger = logger;
            this._feeTemplateService = feeTemplateService;
        }
        #region HttpPost Endpoint
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFeeTemplateDto createFeeTemplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feeTemplate = _mapper.Map<FeeTemplate>(createFeeTemplate);
            var (succes, errorMessage, feeTemplateId) = await _feeTemplateService.CreateAsync(feeTemplate);

            if (!succes)
            {
                _logger.LogWarning($"Failed to create feeTemplate for courseId {feeTemplate.CourseId}");
                return BadRequest($"Error Meaage : {errorMessage}");
            }

            return CreatedAtAction(nameof(GetById), new { id = feeTemplate.FeeTemplateId }, feeTemplate);
        }
        #endregion

        #region HttpGet Endpoint
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var feeTemplates = await _feeTemplateService.GetAllAsync();
            return Ok(feeTemplates);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var feeTemplate = await _feeTemplateService.GetByIdAsync(id);
            if (feeTemplate == null)
            {
                _logger.LogWarning($"feeTemplate with Id {id} not found");
                return NotFound();
            }
            return Ok(feeTemplate);
        }
        #endregion

        #region HttpPut Endpoint
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateFeeTemplateDto updateFeeTemplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feeTemplate = _mapper.Map<FeeTemplate>(updateFeeTemplate);
            var isUpdated = await _feeTemplateService.UpdateAsync(id, feeTemplate);

            if (!isUpdated)
            {
                _logger.LogWarning($"Failed to update FeeTemplate with Id {id}");
                return BadRequest("Update failed");
            }

            return Ok("Update successful");
        }
        #endregion

        #region HttpDelete Endpoint
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var isDeleted = await _feeTemplateService.DeleteAsync(id);

            if (!isDeleted)
            {
                _logger.LogWarning($"Failed to delete FeeTemplate with Id {id}");
                return BadRequest("Delete failed");
            }

            return Ok("Delete successful");
        }
        #endregion
    }
}
