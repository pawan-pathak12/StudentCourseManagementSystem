using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentCourseManagement.Tests.Unit.Common;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders;

namespace StudentCourseManagement.Tests.Unit.Services.AcademicModule.Courses
{
    [TestClass]
    public class CourseServiceTest_Delete : CourseServiceTestBase
    {
        [TestMethod]
        public async Task Delete_WithExistingId_ReturnTrue()
        {
            //Arrange 
            var course = new CourseBuilder()
                .Build();

            var courseId = await _courseRepository.AddAsync(course);

            //Act 
            var isDeleted = await _courseService.DeleteAsync(courseId);

            //Assert
            Assert.IsTrue(isDeleted);
            var courseData = await _courseRepository.GetByIdAsync(courseId);
            Assert.IsNull(courseData);
        }


        [TestMethod]
        public async Task Delete_WithNonExistingId_ReturnFalse()
        {
            //Arrange 
            //Assume Courseid 1234 dont exists 
            int id = 1234;
            //Act 
            var isDeleted = await _courseService.DeleteAsync(id);
            //ASsert

            Assert.IsFalse(isDeleted);
        }
    }
}
