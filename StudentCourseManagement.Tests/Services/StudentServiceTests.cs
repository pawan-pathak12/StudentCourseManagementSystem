using StudentCourseManagement.Business.DTOs.Student;
using StudentCourseManagement.Tests.Common;

namespace StudentCourseManagement.Tests.Services
{
    [TestClass]
    public class StudentServiceTests : StudentServiceTestBase
    {
        [TestMethod]
        public void CreateStudent_Valid_ReturnsTrue()
        {
            var dto = new CreateStudentDto
            {
                Name = "Ram",
                Email = "ram@gmail.com"
            };

            var result = _studentService.Create(dto);

            Assert.IsTrue(result);
        }
    }
}
