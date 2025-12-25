using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Common;

namespace StudentCourseManagement.Tests.Services.Enrollments
{
    [TestClass]
    public class EnrollmentServiceTests_Get : EnrollmentServiceTestBase
    {
        [TestMethod]
        public async Task Get_ReturnsListofExistingEnrollments()
        {
            var enrollment = new Enrollment
            {

            };

            var enrollments = _service.GetAllEnrollmentsAsync();

            Assert.IsNotNull(enrollments);

        }
    }
}
