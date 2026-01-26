using StudentCourseManagement.Tests.Common.Builders;
using StudentCourseManagement.Tests.Unit.Common;

namespace StudentCourseManagement.Tests.Unit.Services.AcademicModule.Courses
{
    [TestClass]
    public class CourseServiceTest_Get : CourseServiceTestBase
    {
        [TestMethod]
        public async Task GetAll_Returns_ListOfCourses()
        {
            //Arrange 
            var course = new CourseBuilder()
                .Build();

            await _courseRepository.AddAsync(course);

            //Act 

            var courses = await _courseService.GetAllAsync();
            //Assert
            Assert.IsNotNull(courses);
            Assert.AreNotEqual(0, _courseRepository._courses.Count());
        }


        [TestMethod]
        public async Task GetById_WithExistingId_ReturnOneCourse()
        {
            //Arrange 
            var course = new CourseBuilder()
                .Build();

            await _courseRepository.AddAsync(course);
            //Act 

            var exisitngCourse = await _courseService.GetByIdAsync(course.CourseId);

            //Assert
            Assert.IsNotNull(exisitngCourse);
            Assert.AreEqual(course.CourseId, exisitngCourse.CourseId);
        }

        [TestMethod]
        public async Task GetAllById_WithNonExistingId_ReturnNull()
        {
            //assme course id 99 dont exists 
            int id = 99;
            //Act 

            var exisitngCourse = await _courseService.GetByIdAsync(id);
            //Assert
            Assert.IsNull(exisitngCourse);
        }

    }
}
