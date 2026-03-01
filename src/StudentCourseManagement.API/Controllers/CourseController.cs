using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCourseManagement.Application.DTOs.Courses;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<CourseController> _logger;
        private readonly IMapper _mapper;

        public CourseController(
            ICourseService courseService,
            ILogger<CourseController> logger,
            IMapper mapper)
        {
            _courseService = courseService;
            _logger = logger;
            _mapper = mapper;
        }

        #region httpPost

        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseDto createCourseDto)
        {
            var course = _mapper.Map<Course>(createCourseDto);
            var (success, errorMessage, courseId) = await _courseService.CreateAsync(course);
            if (!success)
            {
                return BadRequest($"Error Message : {errorMessage}");
            }
            var responseDto = _mapper.Map<CourseResponseDto>(course);

            _logger.LogInformation("Course created successfully ");

            return CreatedAtAction(nameof(GetById), new { id = courseId }, createCourseDto);


        }

        #endregion

        #region httpget
        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all active courses.");

                var courses = await _courseService.GetAllAsync();
                var responseDtos = _mapper.Map<IEnumerable<CourseResponseDto>>(courses);

                return Ok(responseDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all courses.");
                return StatusCode(500, "An error occurred while retrieving courses.");
            }
        }

        // GET: api/course/5
        [HttpGet("{id:int}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching course with ID: {CourseId}", id);

                var course = await _courseService.GetByIdAsync(id);

                if (course == null)
                {
                    _logger.LogWarning("Course with ID {CourseId} not found.", id);
                    return NotFound($"Course with ID {id} not found.");
                }

                var responseDto = _mapper.Map<CourseResponseDto>(course);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching course with ID: {CourseId}", id);
                return StatusCode(500, "An error occurred while retrieving the course.");
            }
        }
        #endregion

        [HttpPut("{id:int}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDto updateCourseDto)
        {

            if (id != updateCourseDto.CourseId)
            {
                _logger.LogWarning("Route ID {RouteId} does not match DTO ID {DtoId}", id, updateCourseDto.CourseId);
                return BadRequest("Course ID in route does not match ID in body.");
            }

            var course = _mapper.Map<Course>(updateCourseDto);
            var success = await _courseService.UpdateAsync(id, course);

            if (!success)
            {
                _logger.LogWarning("Course with ID {CourseId} not found for update.", id);
                return NotFound($"Course with ID {id} not found.");
            }

            return NoContent();

        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _courseService.DeleteAsync(id);

                if (!success)
                {
                    _logger.LogWarning("Course with ID {CourseId} not found for deletion.", id);
                    return NotFound($"Course with ID {id} not found.");
                }
                _logger.LogInformation("Course with ID {CourseId} soft-deleted successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting course with ID: {CourseId}", id);
                return StatusCode(500, "An error occurred while deleting the course.");
            }
        }
    }
}