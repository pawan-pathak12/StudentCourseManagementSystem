using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Unit.Common;

namespace StudentCourseManagement.Tests.Unit.Services.AcademicModule.Students
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
            await _studentService.CreateAsync(student);

            //Assert
            Assert.AreEqual(1, _repository._students.Count());
        }

        [TestMethod]
        public async Task CreateStudent_WithDuplicateEmail_ReturnFalse()
        {
            //Arrange 
            var student = new Student
            {
                StudentId = 1,
                Name = "Pawan",
                Address = "Haldibari",
                Email = "email"
            };
            await _studentService.CreateAsync(student);

            var student2 = new Student
            {
                StudentId = 1,
                Name = "Ram",
                Address = "Haldibari",
                Email = "email"
            };

            //Act 
            var isCreated = await _studentService.CreateAsync(student2);

            //Assert 
            Assert.IsFalse(isCreated);
        }
    }
}
