using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Common;

namespace StudentCourseManagement.Tests.Services.Enrollments
{
    [TestClass]
    public class EnrollmentServiceTests_Get : EnrollmentServiceTestBase
    {
        [TestMethod]
        public async Task Get_ReturnsListof_ExistingEnrollments()
        {
            //Arrange 
            var enrollment = new Enrollment
            {
                CourseId = 1,
                StudentId = 1
            };

            await _service.CreateAsync(enrollment);

            var enrollment2 = new Enrollment
            {
                CourseId = 1,
                StudentId = 1
            };

            await _service.CreateAsync(enrollment2);

            //Act 
            var enrollments = await _service.GetAllAsync();

            //Assert 
            Assert.IsNotNull(enrollments);
            Assert.AreEqual(enrollments.Count(), _repository._enrollments.Count());
        }

        /*   [TestMethod]
           public async Task GetAll_IfNoExisingData_ReturnsNull()
           {
               var enrollments = await _service.GetAllEnrollmentsAsync();

               Assert.IsNull(enrollments);
           }*/
    }
}
