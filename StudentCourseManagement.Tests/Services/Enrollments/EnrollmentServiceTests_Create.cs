using StudentCourseManagement.Business.DTOs.Enrollments;

namespace StudentCourseManagement.Tests.Services.Enrollments
{
    [TestClass]
    public class EnrollmentServiceTests_Create
    {
        [TestMethod]
        public async Task Create_WithValidData_CreateEnrollment()
        {
            var enrollmentDto = new CreateEnrollmentDto
            {
                CourseId = 1,
                StudentId = 1
            };

        }
    }
}
