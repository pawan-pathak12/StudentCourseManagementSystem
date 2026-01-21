using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentCourseManagement.Application.DTOs.Students;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {


        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentService studentService, IMapper mapper, ILogger<StudentController> logger)
        {
            this._studentService = studentService;
            this._mapper = mapper;
            this._logger = logger;
        }


        #region HttpPost

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateStudentDto createStudentDto)
        {
            var student = _mapper.Map<Student>(createStudentDto);
            var (success, errorMessage, studentId) = await _studentService.CreateAsync(student);
            if (!success)
            {
                return BadRequest($"Error Message : {errorMessage}");
            }

            _logger.LogInformation("API Response : Created now student data");
            return CreatedAtAction(nameof(GetByIdAsync), new { id = studentId }, createStudentDto);
        }
        #endregion

        #region HttpGet

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var result = await _studentService.GetAllAsync();
            _logger.LogInformation("API resposne : returning student all record ");

            return Ok(result);


        }

        #region GetById
        [HttpGet("{id}")]

        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            _logger.LogInformation($"API request : GET student with ID {id}");

            var student = await _studentService.GetByIdAsync(id);
            _logger.LogInformation("API Resposne : returning data");
            var studentDto = _mapper.Map<StudentResponseDto>(student);

            return Ok(studentDto);
        }

        #endregion

        #endregion

        #region HttpPut

        [HttpPut]

        public async Task<IActionResult> UpdateAsync([FromBody] UpdateStudentDto updateStudentDto)
        {
            var student = _mapper.Map<Student>(updateStudentDto);

            var isUpdated = await _studentService.UpdateAsync(student.StudentId, student);

            if (!isUpdated)
            {
                _logger.LogError($"Update failed for student id {student.StudentId}");
                return BadRequest("Update failed");
            }

            return Ok("Update is successful");
        }
        #endregion


        #region HttpDelete 

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {

            var isDeleted = await _studentService.DeleteAsync(id);
            if (!isDeleted)
            {
                _logger.LogInformation($"API Request : soft delete failed for student with Id {id}");
                return BadRequest("Failed to delete");
            }
            return Ok("Delete is Successful");
        }
        #endregion
    }
}
