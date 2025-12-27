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
            await _studentService.CreateAsync(studentData);


            var student = new Student
            {
                StudentId = 1,
                Name = "Ram Nath",
                Address = "Ktm"
            };

            var isUpdated = await _studentService.UpdateAsync(studentData.StudentId, student);

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
            await _studentService.CreateAsync(studentData);


            //assume id 111 don't exists
            int id = 111;
            var student = new Student
            {
                StudentId = 111,
                Name = "Ram Nath",
                Address = "Ktm"
            };

            var isUpdated = await _studentService.UpdateAsync(id, student);

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
            await _studentService.CreateAsync(studentData);


            var student = new Student
            {
                StudentId = 111,
                Name = "Ram Nath",
                Address = "Ktm"
            };

            var isUpdated = await _studentService.UpdateAsync(studentData.StudentId, student);

            Assert.IsFalse(isUpdated);

        }

        [TestMethod]
        public async Task Update_WithInactiveStudent_ReturnsFalse()
        {
            // Arrange
            var student = new Student
            {
                Name = "Ram",
                Address = "Ktm",
                IsActive = false
            };

            await _studentService.CreateAsync(student);

            // Act
            var result = await _studentService.UpdateAsync(1, new Student
            {
                Name = "Ram Nath",
                Address = "Ktm"
            });

            // Assert
            Assert.IsFalse(result);
        }



    }
}
