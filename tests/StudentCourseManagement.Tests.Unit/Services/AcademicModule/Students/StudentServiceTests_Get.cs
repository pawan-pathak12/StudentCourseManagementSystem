using StudentCourseManagement.Tests.Common.Builders;
using StudentCourseManagement.Tests.Unit.Common;

namespace StudentCourseManagement.Tests.Unit.Services.AcademicModule.Students
{
    [TestClass]
    public class StudentServiceTests_Get : StudentServiceTestBase
    {
        [TestMethod]
        public async Task GetAll_ShouldReturns_listOfStudent()
        {
            //Arrange 
            var student = new StudentBuilder()
                .Build();

            await _studentService.CreateAsync(student);
            //Act

            var students = _studentService.GetAllAsync();

            //Assert
            Assert.IsNotNull(students);
        }

        [TestMethod]
        public async Task GetById_WithExistingId_ShouldReturnStudentData()
        {
            //Arrange 
            var studentData = new StudentBuilder()
                .Build();
            var (success, erroMessage, studentId) = await _studentService.CreateAsync(studentData);

            //Act
            var student = await _studentService.GetByIdAsync(studentId);
            //Assert
            Assert.IsNotNull(student);
        }

        [TestMethod]
        public async Task GetById_WithNonExistingId_ReturnNull()
        {
            //Arrange 
            var studentData = new StudentBuilder()
                .Build();
            await _studentService.CreateAsync(studentData);
            //Assume no  Id 111 exists 
            int id = 111;
            //Act 
            var student = await _studentService.GetByIdAsync(id);

            //Assert
            Assert.IsNull(student);
        }
    }
}
