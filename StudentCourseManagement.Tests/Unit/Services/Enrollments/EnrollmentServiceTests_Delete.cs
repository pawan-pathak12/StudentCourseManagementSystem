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
            var enrollment = new Enrollment
            {
                StudentId = 1,
                CourseId = 1
            };

            var enrollmentId = await _service.CreateAsync(enrollment);

            //Act 
            var isDeleted = await _service.DeleteAsync(enrollmentId);

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
