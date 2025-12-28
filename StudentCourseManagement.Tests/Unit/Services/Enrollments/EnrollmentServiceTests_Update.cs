using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Unit.Common;

namespace StudentCourseManagement.Tests.Unit.Services.Enrollments
{
    [TestClass]
    public class EnrollmentServiceTests_Update : EnrollmentServiceTestBase
    {
        [TestMethod]
        public async Task Update_WihExisitngEnrollmentId_ReturnsTrue()
        {
            var enrollment = new Enrollment
            {
                CourseId = 1,
                StudentId = 1
            };

            var enrollmentId = await _service.CreateAsync(enrollment);

            var enrollment2 = new Enrollment
            {
                EnrollmentId = enrollmentId,
                IsActive = false,
                CancellationReason = "testing "
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
