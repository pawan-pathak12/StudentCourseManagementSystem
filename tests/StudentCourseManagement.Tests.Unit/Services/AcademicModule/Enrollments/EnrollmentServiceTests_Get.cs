using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentCourseManagement.Tests.Unit.Common;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders;

namespace StudentCourseManagement.Tests.Unit.Services.AcademicModule.Enrollments
{
    [TestClass]
    [DoNotParallelize]
    public class EnrollmentServiceTests_Get : EnrollmentServiceTestBase
    {
        [TestMethod]
        public async Task Get_ReturnsListof_ExistingEnrollments()
        {
            //Arrange 
            var student = new StudentBuilder()
              .Build();
            var course = new CourseBuilder()
                   .Build();

            var studentId = await _studentRepository.AddAsync(student);
            var courseId = await _courseRepository.AddAsync(course);

            var enrollment = new EnrollmentBuilder()
                .WithStudentId(studentId).WithCourseId(courseId).Build();

            await _service.CreateAsync(enrollment);

            //Act 
            var enrollments = await _service.GetAllAsync();

            //Assert 
            Assert.IsNotNull(enrollments);
            Assert.AreEqual(enrollments.Count(), _repository._enrollments.Count());
        }


    }
}
