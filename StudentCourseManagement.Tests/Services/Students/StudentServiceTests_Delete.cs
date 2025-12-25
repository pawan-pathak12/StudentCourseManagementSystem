using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Common;

namespace StudentCourseManagement.Tests.Services.Students
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
                Address = "Ktm"
            };
            await _studentService.Create(student);

            //Assume id 1 exists
            int id = 1;

            var isDeleted = await _studentService.Delete(1);

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
                Address = "Ktm"
            };
            await _studentService.Create(student);



            //Assume id 1111 don't exists
            int id = 1111;

            var isDeleted = await _studentService.Delete(id);

            Assert.IsFalse(isDeleted);
        }
    }
}
