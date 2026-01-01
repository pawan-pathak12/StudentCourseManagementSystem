using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Unit.Common;

namespace StudentCourseManagement.Tests.Unit.Services.Enrollments
{
    [TestClass]
    public class EnrollmentServiceTests_Delete : EnrollmentServiceTestBase
    {
        [TestMethod]
        public async Task Delete_WithExistingEnrollmentId_ReturnTrue()
        {


            //Arrange 
            var student = new Student
            {
                Name = "Ram",
                Address = "haldibari"
            };

            var course = new Course
            {
                Capacity = 10,
                Code = "CA002"
            };
            await _studentRepository.AddAsync(student);
            await _courseRepository.AddAsync(course);
            var enrollment = new Enrollment
            {
                StudentId = 1,
                CourseId = 1
            };

            await _service.CreateAsync(enrollment);

            //assume enrollment id =1 exists 
            int id = 1;

            //Act 
            var isDeleted = await _service.DeleteAsync(id);

            Assert.IsTrue(isDeleted);

        }

        [TestMethod]
        public async Task Delete_WithNonExistingEnrollmentId_ReturnFalse()
        {
            //Assume Enrollment Id 555 don't exists
            int enrollmentId = 555;

            var isDeleted = await _service.DeleteAsync(enrollmentId);

            Assert.IsFalse(isDeleted);

        }
    }
}
