using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Unit.Common;

namespace StudentCourseManagement.Tests.Unit.Services.AcademicModule.Students
{
    [TestClass]
    public class StudentServiceTests_Get : StudentServiceTestBase
    {
        [TestMethod]
        public async Task GetAll_ReturnslistOfStudent()
        {
            //Arrange 
            var student = new Student
            {
                StudentId = 1,
                Name = "Ram",
                Address = "Ktm"
            };
            await _studentService.CreateAsync(student);

            var students = _studentService.GetAllAsync();

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
            await _studentService.CreateAsync(studentData);


            //Assume Id 1 exists 
            //     int id = 1;

            var student = await _studentService.GetByIdAsync(studentData.StudentId);

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
            await _studentService.CreateAsync(studentData);


            //Assume no  Id 111 exists 
            int id = 111;

            var student = await _studentService.GetByIdAsync(id);

            Assert.IsNull(student);
        }
    }
}
