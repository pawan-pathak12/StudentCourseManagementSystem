using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Unit.Common;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders;

namespace StudentCourseManagement.Tests.Unit.Services.AcademicModule.Enrollments
{
    [TestClass]
    public class EnrollmentServiceTests_Update : EnrollmentServiceTestBase
    {
        [TestMethod]
        public async Task Update_WihExisitngEnrollmentId_ReturnsTrue()
        {
            var student = new StudentBuilder()
               .Build();
            var course = new CourseBuilder()
                   .Build();

            var studentId = await _studentRepository.AddAsync(student);
            var courseId = await _courseRepository.AddAsync(course);



            await _studentRepository.AddAsync(student);
            await _courseRepository.AddAsync(course);

            var enrollment = new EnrollmentBuilder()
               .WithStudentId(studentId).WithCourseId(courseId).Build();

            var enrollmentId = await _repository.AddAsync(enrollment);

            var enrollment2 = new EnrollmentBuilder()
               .WithStudentId(studentId).WithCourseId(courseId).WithEnrollmentId(enrollmentId).WithCancellationReason("Nothing").Build();

            var isUpdated = await _service.UpdateAsync(enrollmentId, enrollment2);

            Assert.IsTrue(isUpdated);
            var enrollmentData = await _repository.GetByIdAsync(enrollmentId);
            Assert.IsNotNull(enrollmentData);
            Assert.AreEqual(enrollment2.CancellationReason, enrollmentData.CancellationReason);
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
