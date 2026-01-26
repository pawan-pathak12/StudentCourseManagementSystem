using StudentCourseManagement.Tests.Common.Builders;
using StudentCourseManagement.Tests.Unit.Common;

namespace StudentCourseManagement.Tests.Unit.Services.AcademicModule.Students
{
    [TestClass]
    public class StudentServiceTests_Create : StudentServiceTestBase
    {

        [TestMethod]
        public async Task CreateStudent_ValidStudent_ShouldSucceed()
        {
            //Arrange 

            var student = new StudentBuilder()
                .Build();

            //Act 
            var (success, errorMessage, studentId) = await _studentService.CreateAsync(student);

            //Assert
            Assert.IsTrue(success);
            Assert.IsNull(errorMessage);
        }

        [TestMethod]
        public async Task CreateStudent_WithDuplicateEmail_ShouldReturnError()
        {
            //Arrange 
            var email = "testemail@gmail.com";

            var student = new StudentBuilder()
                .WithEmail(email).Build();
            await _studentService.CreateAsync(student);

            var student2 = new StudentBuilder()
                  .WithEmail(email).Build();
            //Act 
            var (success, errorMessage, studentId) = await _studentService.CreateAsync(student2);

            //Assert 
            Assert.IsFalse(success);
            Assert.IsNotNull(errorMessage);
        }
    }
}
