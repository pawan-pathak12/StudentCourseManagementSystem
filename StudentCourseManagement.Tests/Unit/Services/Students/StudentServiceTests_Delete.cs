using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Unit.Common;

namespace StudentCourseManagement.Tests.Unit.Services.Students
{
    [TestClass]
    public class StudentServiceTests_Delete : StudentServiceTestBase
    {
        [TestMethod]
        public async Task Delete_WithExistingId_ReturnTrueAfterDeleting()
        {
            //Arrange 
            var student = new Student
            {
                StudentId = 1,
                Name = "Ram",
                Address = "Ktm",
                IsActive = true
            };
            await _studentService.CreateAsync(student);

            //Assume id 1 exists
            int id = 1;

            var isDeleted = await _studentService.DeleteAsync(id);
            Console.WriteLine(isDeleted);

            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public async Task Delete_WitNonExistingId_ReturnFalseAfterDeleting()
        {
            //Arrange 
            var student = new Student
            {
                StudentId = 1,
                Name = "Ram",
                Address = "Ktm",
                IsActive = true
            };
            await _studentService.CreateAsync(student);

            //Assume id 1111 don't exists
            int id = 1111;

            var isDeleted = await _studentService.DeleteAsync(id);

            Assert.IsFalse(isDeleted);
        }
    }
}
