using StudentCourseManagement.Tests.Unit.Common;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders;

namespace StudentCourseManagement.Tests.Unit.Services.AcademicModule.Courses
{
    [TestClass]
    public class CourseServiceTest_Update : CourseServiceTestBase
    {
        [TestMethod]
        public async Task Update_WithExistingID_ReturnTrue()
        {
            //Arrange 
            var course = new CourseBuilder()
                .Build();

            var courseId = await _courseRepository.AddAsync(course);

            var course2 = new CourseBuilder()
                .WithCourseId(courseId).WithCredits(5).WithDescription("Testing Update").Build();

            //Act 
            var isUpdated = await _courseService.UpdateAsync(courseId, course2);
            Assert.IsTrue(isUpdated);

            var courseData = await _courseRepository.GetByIdAsync(courseId);
            Assert.AreEqual(course2.Credits, courseData.Credits);
            Assert.AreEqual(course2.Description, courseData.Description);
        }


        [TestMethod]
        public async Task Update_WithNonExistingID_ReturnFalse()
        {
            //Arrange 
            //assume id 111 dont exists

            int id = 111;
            var course2 = new CourseBuilder()
                .Build();
            //Act 
            var isUpdated = await _courseService.UpdateAsync(id, course2);

            //Assert
            Assert.IsFalse(isUpdated);
        }

    }
}
