using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Common;

namespace StudentCourseManagement.Tests.Services.Students
{
    [TestClass]
    public class StudentServiceTests_Create : StudentServiceTestBase
    {

        [TestMethod]
        public async Task CreateStudent_ValidStudent_AddStudent()
        {
            //Arrange 
            var student = new Student
            {
                StudentId = 1,
                Name = "Pawan",
                Address = "Haldibari",
                Email = "email"
            };

            //Act 
            await _studentService.Create(student);

            //Assert
            Assert.AreEqual(1, _repository._students.Count());
        }
    }
}
