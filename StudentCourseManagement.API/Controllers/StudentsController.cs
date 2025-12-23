using Microsoft.AspNetCore.Mvc;
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

        #region HttpGet

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = _studentService.GetAll();
            return Ok(result);
        }

        #endregion
    }
}
