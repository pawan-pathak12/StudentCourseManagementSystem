using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentCourseManagement.Tests.Unit.Common;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders;

namespace StudentCourseManagement.Tests.Unit.Services.AcademicModule.Courses
{
    [TestClass]
    public class CourseServiceTest_Create : CourseServiceTestBase
    {
        [TestMethod]
        public async Task CreateCourse_WithValidData_ShouldSucessed()
        {
            //Arrange 
            var course = new CourseBuilder()
                .Build();

            //Act 
            var (success, erroMessage, courseId) = await _courseService.CreateAsync(course);
            //Assert
            Assert.IsTrue(success);
            Assert.IsTrue(courseId > 0);
        }

        [TestMethod]
        public async Task CreateAsync_WithDuplicateCode_ReturnsFalse()
        {
            //Arrange 
            var code = "A112A";
            var course = new CourseBuilder()
                .WithCode(code).Build();

            await _courseRepository.AddAsync(course);

            var course2 = new CourseBuilder()
                    .WithCode(code).Build();

            //Act
            var (success, erroMessage, courseId) = await _courseService.CreateAsync(course2);

            //Assert
            Assert.IsFalse(success);
            Assert.IsNotNull(erroMessage);
        }

        [TestMethod]
        public async Task CreateAsync_WithDuplicateTitle_ReturnsFalse()
        {
            //Arrange 
            var title = "Introduction to Computer Science";
            var course = new CourseBuilder()
            .WithTitle(title).Build();

            await _courseService.CreateAsync(course);

            var course2 = new CourseBuilder()
            .WithTitle(title).Build();

            //Act
            var (success, erroMessage, courseId) = await _courseService.CreateAsync(course2);

            //Assert
            Assert.IsFalse(success);
            Assert.IsNotNull(erroMessage);
        }
    }
}
