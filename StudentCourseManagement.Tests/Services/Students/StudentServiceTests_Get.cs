using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Common;

namespace StudentCourseManagement.Tests.Services.Students
{
    [TestClass]
    public class StudentServiceTests_Get : StudentServiceTestBase
    {
        [TestMethod]
        public async void GetAll_ReturnslistOfStudent()
        {
            //Arrange 
            var student = new Student
            {
                StudentId = 1,
                Name = "Ram",
                Address = "Ktm"
            };
            await _studentService.Create(student);

            var students = _studentService.GetAll();

            Assert.IsNotNull(students);
        }

        [TestMethod]
        public async Task GetById_WithExistingId_ReturnOneStudentData()
        {
            //Arrange 
            var studentData = new Student
            {
                StudentId = 1,
                Name = "Ram",
                Address = "Ktm"
            };
            await _studentService.Create(studentData);


            //Assume Id 1 exists 
            int id = 1;

            var student = await _studentService.GetById(id);

            Assert.IsNotNull(student);

            //    Assert.AreEqual(id, student.Id);
        }

        [TestMethod]
        public async Task GetById_WithNonExistingId_ReturnNull()
        {
            //Arrange 
            var studentData = new Student
            {
                StudentId = 1,
                Name = "Ram",
                Address = "Ktm"
            };
            await _studentService.Create(studentData);


            //Assume no  Id 111 exists 
            int id = 111;

            var student = await _studentService.GetById(id);

            Assert.IsNull(student);
        }
    }
}
