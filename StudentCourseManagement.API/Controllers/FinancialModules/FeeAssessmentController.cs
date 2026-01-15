using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentCourseManagement.API.DTOs.FInancialModule.FeeAssessments;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.API.Controllers.FinancialModules
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeeAssessmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFeeAssessmentService _feeAssessmentService;
        private readonly ILogger<FeeAssessmentController> _logger;

        public FeeAssessmentController(IMapper mapper, IFeeAssessmentService feeAssessmentService, ILogger<FeeAssessmentController> logger)
        {
            _mapper = mapper;
            _feeAssessmentService = feeAssessmentService;
            _logger = logger;
        }

        #region HttpPost Endpoint
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFeeAssessmentDto createFeeAssessment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feeAssessment = _mapper.Map<FeeAssessment>(createFeeAssessment);
            var isCreated = await _feeAssessmentService.CreateAsync(feeAssessment);

            if (!isCreated)
            {
                _logger.LogWarning("Failed to create FeeAssessment for EnrollmentId {EnrollmentId}, CourseId {CourseId}",
                    feeAssessment.EnrollmentId, feeAssessment.CourseId);
                return BadRequest("Failed to create FeeAssessment");
            }

            return CreatedAtAction(nameof(GetById), new { id = feeAssessment.FeeAssessmentId }, feeAssessment);
        }
        #endregion

        #region HttpGet Endpoint
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var feeAssessments = await _feeAssessmentService.GetAllAsync();
            return Ok(feeAssessments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var feeAssessment = await _feeAssessmentService.GetByIdAsync(id);
            if (feeAssessment == null)
            {
                _logger.LogWarning("FeeAssessment with Id {FeeAssessmentId} not found", id);
                return NotFound();
            }
            return Ok(feeAssessment);
        }
        #endregion

        #region HttpPut Endpoint
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateFeeAssessmentDto updateFeeAssessment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feeAssessment = _mapper.Map<FeeAssessment>(updateFeeAssessment);
            var isUpdated = await _feeAssessmentService.UpdateAsync(id, feeAssessment);

            if (!isUpdated)
            {
                _logger.LogWarning("Failed to update FeeAssessment with Id {FeeAssessmentId}", id);
                return BadRequest("Update failed");
            }

            return Ok("Update successful");
        }
        #endregion

        #region HttpDelete Endpoint
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var isDeleted = await _feeAssessmentService.DeleteAsync(id);

            if (!isDeleted)
            {
                _logger.LogWarning("Failed to delete FeeAssessment with Id {FeeAssessmentId}", id);
                return BadRequest("Delete failed");
            }

            return Ok("Delete successful");
        }
        #endregion
    }
}