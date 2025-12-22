using Microsoft.AspNetCore.Mvc;
using StudentCourseManagement.Business.DTOs.Student;
using StudentCourseManagement.Business.Interfaces.Services;

namespace StudentCourseManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            this._studentService = studentService;
        }


        [HttpPost]
        public IActionResult Create(CreateStudentDto dto)
        {
            if (!_studentService.Create(dto))
                return Conflict();

            return Ok();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _studentService.GetAll();
            return Ok(result);
        }
    }
}
