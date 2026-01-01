using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Unit.Common;

namespace StudentCourseManagement.Tests.Unit.Services.Enrollments
{
    [TestClass]
    public class EnrollmentServiceTests_Create : EnrollmentServiceTestBase
    {

        [TestMethod]
        public async Task CreateAsync_WithNewLogic_CreateEnrollment()
        {
            var student = new Student
            {
                Address = "testing"
            };

            var course = new Course
            {
                Capacity = 1
            };

            var enrollment = new Enrollment
            {
                StudentId = 1,
                CourseId = 1
            };

            await _studentRepository.AddAsync(student);
            await _courseRepository.AddAsync(course);

            var isCreated = await _service.CreateAsync(enrollment);

            Assert.IsTrue(isCreated);
        }
    }
}
