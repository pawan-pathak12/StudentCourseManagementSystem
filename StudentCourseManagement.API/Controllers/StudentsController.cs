using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentCourseManagement.Business.DTOs.Student;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;

        public StudentsController(IStudentService studentService, IMapper mapper)
        {
            this._studentService = studentService;
            this._mapper = mapper;
        }


        #region HttpPost

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateStudentDto createStudentDto)
        {
            var student = _mapper.Map<Student>(createStudentDto);
            await _studentService.Create(student);
            return Ok("Student Record Added");


        }
        #endregion

        #region HttpGet

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = _studentService.GetAll();
            return Ok(result);
        }

        #region GetById
        [HttpGet("{id}")]

        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var student = await _studentService.GetById(id);
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

            var isUpdated = await _studentService.Update(student.Id, student);

            if (!isUpdated)
            {
                return BadRequest("Update failed");
            }

            return Ok("Update is successful");
        }
        #endregion


        #region HttpDelete 

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
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
