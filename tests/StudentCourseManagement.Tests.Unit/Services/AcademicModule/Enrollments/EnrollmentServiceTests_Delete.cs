using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentCourseManagement.Tests.Unit.Common;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders;

namespace StudentCourseManagement.Tests.Unit.Services.AcademicModule.Enrollments
{
    [TestClass]
    public class EnrollmentServiceTests_Delete : EnrollmentServiceTestBase
    {
        [TestMethod]
        public async Task Delete_WithExistingEnrollmentId_ReturnTrue()
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

            var enrollmentId = await _repository.AddAsync(enrollment);

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
