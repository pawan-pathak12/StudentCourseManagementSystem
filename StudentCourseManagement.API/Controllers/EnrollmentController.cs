using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentCourseManagement.Application.DTOs.DTOs.Enrollments;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IMapper _mapper;
        private readonly ILogger<EnrollmentController> _logger;

        public EnrollmentController(
            IEnrollmentService enrollmentService,
            IMapper mapper,
            ILogger<EnrollmentController> logger)
        {
            _enrollmentService = enrollmentService;
            _mapper = mapper;
            _logger = logger;
        }

        #region POST - Create Enrollment
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEnrollmentDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var enrollment = _mapper.Map<Enrollment>(createDto);
                var (success, errorMessage, enrollmentId) = await _enrollmentService.CreateAsync(enrollment);
                if (!success)
                {
                    return BadRequest($"Error Message : {errorMessage}");
                }
                var responseDto = _mapper.Map<EnrollmentResponseDto>(enrollment);
                return CreatedAtAction(nameof(GetById), new { id = enrollmentId }, createDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating enrollment for Student {StudentId} and Course {CourseId}",
                    createDto.StudentId, createDto.CourseId);
                return StatusCode(500, "An error occurred while creating the enrollment.");
            }
        }
        #endregion

        #region GET - Get All Enrollments
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var enrollments = await _enrollmentService.GetAllAsync();
                var responseDtos = _mapper.Map<IEnumerable<EnrollmentResponseDto>>(enrollments);

                return Ok(responseDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all enrollments.");
                return StatusCode(500, "An error occurred while retrieving enrollments.");
            }
        }
        #endregion

        #region GET - Get Enrollment By Id

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var enrollment = await _enrollmentService.GetByIdAsync(id);

                if (enrollment == null)
                {
                    return NotFound($"Enrollment with ID {id} not found.");
                }

                var responseDto = _mapper.Map<EnrollmentResponseDto>(enrollment);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving enrollment with ID {EnrollmentId}", id);
                return StatusCode(500, "An error occurred while retrieving the enrollment.");
            }
        }
        #endregion

        #region PUT - Update Enrollment
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEnrollmentDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != updateDto.EnrollmentId)
            {
                return BadRequest("Enrollment ID in route does not match ID in body.");
            }

            try
            {
                var enrollment = _mapper.Map<Enrollment>(updateDto);
                var success = await _enrollmentService.UpdateAsync(id, enrollment);

                if (!success)
                {
                    return NotFound($"Enrollment with ID {id} not found.");
                }

                _logger.LogInformation("Enrollment with ID {EnrollmentId} updated successfully", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating enrollment with ID {EnrollmentId}", id);
                return StatusCode(500, "An error occurred while updating the enrollment.");
            }
        }
        #endregion

        #region DELETE - Soft Delete Enrollment

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var isDeleted = await _enrollmentService.DeleteAsync(id);

                if (!isDeleted)
                {
                    return NotFound($"Enrollment with ID {id} not found.");
                }

                _logger.LogInformation("Enrollment with ID {EnrollmentId} soft-deleted successfully", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting enrollment with ID {EnrollmentId}", id);
                return StatusCode(500, "An error occurred while deleting the enrollment.");
            }
        }
        #endregion
    }
}