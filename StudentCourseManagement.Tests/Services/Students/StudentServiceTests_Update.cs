using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Common;

namespace StudentCourseManagement.Tests.Services.Students
{
    [TestClass]
    public class StudentServiceTests_Update : StudentServiceTestBase
    {
        [TestMethod]

        public async Task Update_WithExistingId_ReturnsTrueAfterUpdating()
        {
            //Arrange 
            var studentData = new Student
            {
                StudentId = 1,
                Name = "Ram",
                Address = "Ktm"
            };
            await _studentService.Create(studentData);


            var student = new Student
            {
                StudentId = 1,
                Name = "Ram Nath",
                Address = "Ktm"
            };

            var isUpdated = await _studentService.Update(studentData.Id, student);

            Assert.IsTrue(isUpdated);

        }

        [TestMethod]
        public async Task Update_WithNonExistingId_ReturnsFalseAfterUpdating()
        {
            //Arrange 
            var studentData = new Student
            {
                StudentId = 1,
                Name = "Ram",
                Address = "Ktm"
            };
            await _studentService.Create(studentData);


            //assume id 111 don't exists
            int id = 111;
            var student = new Student
            {
                Id = 111,
                Name = "Ram Nath",
                Address = "Ktm"
            };

            var isUpdated = await _studentService.Update(id, student);

            Assert.IsFalse(isUpdated);

        }

        [TestMethod]
        public async Task Update_IfIdMisMatched_ReturnsFalseAfterUpdating()
        {
            //Arrange 
            var studentData = new Student
            {
                StudentId = 1,
                Name = "Ram",
                Address = "Ktm"
            };
            await _studentService.Create(studentData);


            var student = new Student
            {
                StudentId = 111,
                Name = "Ram Nath",
                Address = "Ktm"
            };

            var isUpdated = await _studentService.Update(studentData.Id, student);

            Assert.IsFalse(isUpdated);

        }


    }
}
