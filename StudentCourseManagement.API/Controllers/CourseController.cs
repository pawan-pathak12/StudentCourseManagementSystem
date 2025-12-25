using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentCourseManagement.Business.DTOs.Courses;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.API.Controllers
{
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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseDto createCourseDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for CreateCourse request.");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating new course with title: {Title}", createCourseDto.Title);

                var course = _mapper.Map<Course>(createCourseDto);
                var createdId = await _courseService.Create(course);

                var responseDto = _mapper.Map<CourseResponseDto>(course);
                responseDto.CourseId = createdId;

                _logger.LogInformation("Course created successfully with ID: {CourseId}", createdId);

                return CreatedAtAction(nameof(GetById), new { id = createdId }, responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating course.");
                return StatusCode(500, "An error occurred while creating the course.");
            }
        }

        #endregion


        #region httpget
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all active courses.");

                var courses = await _courseService.GetAll();
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
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching course with ID: {CourseId}", id);

                var course = await _courseService.GetById(id);

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

        // PUT: api/course/5  (Full update)
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDto updateCourseDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for UpdateCourse request. ID: {CourseId}", id);
                return BadRequest(ModelState);
            }

            if (id != updateCourseDto.CourseId)
            {
                _logger.LogWarning("Route ID {RouteId} does not match DTO ID {DtoId}", id, updateCourseDto.CourseId);
                return BadRequest("Course ID in route does not match ID in body.");
            }

            try
            {
                _logger.LogInformation("Updating course with ID: {CourseId}", id);

                var course = _mapper.Map<Course>(updateCourseDto);
                var success = await _courseService.Update(id, course);

                if (!success)
                {
                    _logger.LogWarning("Course with ID {CourseId} not found for update.", id);
                    return NotFound($"Course with ID {id} not found.");
                }

                _logger.LogInformation("Course with ID {CourseId} updated successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating course with ID: {CourseId}", id);
                return StatusCode(500, "An error occurred while updating the course.");
            }
        }

        // DELETE: api/course/5  (Soft delete)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deleting (soft) course with ID: {CourseId}", id);

                var success = await _courseService.Delete(id);

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