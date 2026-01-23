using StudentCourseManagement.Tests.Unit.Common;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders;

namespace StudentCourseManagement.Tests.Unit.Services.AcademicModule.Students
{
    [TestClass]
    public class StudentServiceTests_Delete : StudentServiceTestBase
    {
        [TestMethod]
        public async Task Delete_WithExistingId_ReturnTrueAfterDeleting()
        {
            //Arrange 

            var student = new StudentBuilder()
                  .Build();

            var (success, errorMessage, studentId) = await _studentService.CreateAsync(student);
            //Act
            var isDeleted = await _studentService.DeleteAsync(studentId);
            //Assert 
            Assert.IsTrue(isDeleted);
            var studentData = await _repository.GetByIdAsync(studentId);
            Assert.IsNull(studentData);
        }

        [TestMethod]
        public async Task Delete_WitNonExistingId_ReturnFalseAfterDeleting()
        {
            //Arrange 
            var student = new StudentBuilder()
                .Build();

            await _studentService.CreateAsync(student);

            //Assume id 1111 don't exists
            int id = 1111;

            var isDeleted = await _studentService.DeleteAsync(id);

            Assert.IsFalse(isDeleted);
        }
    }
}
