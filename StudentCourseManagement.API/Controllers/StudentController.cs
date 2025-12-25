using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentCourseManagement.Business.DTOs.Student;
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
            _logger.LogInformation("API request : POST new student data");

            await _studentService.Create(student);

            _logger.LogInformation("API Response : Created now student data");
            return Ok("Student Record Added");


        }
        #endregion

        #region HttpGet

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("API request : GET student all record ");

            var result = await _studentService.GetAll();
            _logger.LogInformation("API resposne : returning student all record ");

            return Ok(result);


        }

        #region GetById
        [HttpGet("{id}")]

        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            _logger.LogInformation($"API request : GET student with ID {id}");

            var student = await _studentService.GetById(id);
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

            var isUpdated = await _studentService.Update(student.StudentId, student);

            if (!isUpdated)
            {
                return BadRequest("Update failed");
            }

            return Ok("Update is successful");
        }
        #endregion


        #region HttpDelete 

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            _logger.LogInformation($"API Request : Delete student with Id {id}");

            var isDeleted = await _studentService.Delete(id);
            if (!isDeleted)
            {
                return BadRequest("Failed to delete");
            }
            return Ok("Delete is Successful");
        }
        #endregion
    }
}
