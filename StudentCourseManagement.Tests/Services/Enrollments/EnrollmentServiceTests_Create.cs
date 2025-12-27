using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Common;

namespace StudentCourseManagement.Tests.Services.Enrollments
{
    [TestClass]
    public class EnrollmentServiceTests_Create : EnrollmentServiceTestBase
    {
        [TestMethod]
        public async Task Create_WithValidData_CreateEnrollment()
        {
            var enrollment = new Enrollment
            {
                CourseId = 1,
                StudentId = 1
            };

            var enrollmentId = await _service.CreateAsync(enrollment);

            Assert.AreEqual(1, enrollmentId);
        }
    }
}
