using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Unit.Common;

namespace StudentCourseManagement.Tests.Unit.Services.AcademicModule.Enrollments
{
    [TestClass]
    public class EnrollmentServiceTests_Update : EnrollmentServiceTestBase
    {
        [TestMethod]
        public async Task Update_WihExisitngEnrollmentId_ReturnsTrue()
        {
            var student = new Student
            {
                Address = "HAlibari ",
                Name = "Ram Nath"
            };
            var course = new Course
            {
                Capacity = 20,
                Code = "C2001"
            };

            await _studentRepository.AddAsync(student);
            await _courseRepository.AddAsync(course);

            var enrollment = new Enrollment
            {
                CourseId = 1,
                StudentId = 1
            };

            var enrollmentId = await _repository.AddAsync(enrollment);

            var enrollment2 = new Enrollment
            {
                EnrollmentId = 1,
                IsActive = true,
                CancellationReason = "testing ",
                CourseId = enrollment.CourseId,
                StudentId = enrollment.StudentId
            };

            var isUpdated = await _service.UpdateAsync(enrollmentId, enrollment2);

            Assert.IsTrue(isUpdated);
        }


        [TestMethod]
        public async Task Update_WihNonExisitngEnrollmentId_Returnsfalse()
        {
            //let Assume Enrollment with ID 1111 dont exists
            var enrollmentId = 1111;

            var enrollment = new Enrollment
            {
                EnrollmentId = enrollmentId,
                IsActive = false,
                CancellationReason = "testing "
            };

            var isUpdated = await _service.UpdateAsync(enrollmentId, enrollment);

            Assert.IsFalse(isUpdated);
        }
    }

}
