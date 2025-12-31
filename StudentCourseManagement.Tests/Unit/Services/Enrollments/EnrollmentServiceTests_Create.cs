using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Unit.Common;

namespace StudentCourseManagement.Tests.Unit.Services.Enrollments
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

        [TestMethod]
        public async Task CreateAsync_WithNewLogic_CreateEnrollment()
        {
            var student = new Student
            {
                StudentId = 1,
                Address = "testing"
            };

            var course = new Course
            {
                CourseId = 1,
                Capacity = 1
            };

            var enrollment = new Enrollment
            {
                StudentId = 1,
                CourseId = 1
            };

            await _studentRepository.AddAsync(student);
            await _courseRepository.AddAsync(course);

            var enrollmentId = await _service.CreateAsync(enrollment);

            Assert.IsNotNull(enrollmentId);
            Assert.AreNotEqual(0, enrollmentId);
        }
    }
}
