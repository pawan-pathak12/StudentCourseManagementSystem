using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Unit.Common;

namespace StudentCourseManagement.Tests.Unit.Services.Courses
{
    [TestClass]
    public class CourseServiceTest_Get : CourseServiceTestBase
    {
        [TestMethod]
        public async Task GetAll_Returns_ListOfCourses()
        {
            //Arrange 
            var course = new Course
            {
                CourseId = 1,
                Code = "A112A",
                Title = "Introduction to Computer Science",
                Credits = 3,
                Description = "Basic concepts of programming, algorithms, and problem-solving.",
                Instructor = "Dr. Sharma",
                StartDate = new DateTimeOffset(2026, 1, 15, 9, 0, 0, TimeSpan.FromHours(5.75)), // Jan 15, 2026, 9 AM NPT
                EndDate = new DateTimeOffset(2026, 5, 15, 17, 0, 0, TimeSpan.FromHours(5.75)),  // May 15, 2026, 5 PM NPT
                IsActive = true,
                Capacity = 20,
                EnrollmentStartDate = new DateTimeOffset(2025, 12, 25, 8, 0, 0, TimeSpan.FromHours(5.75)), // today
                EnrollmentEndDate = new DateTimeOffset(2026, 1, 10, 23, 59, 0, TimeSpan.FromHours(5.75))   // Jan 10, 2026
            };

            await _courseService.CreateAsync(course);

            //Act 

            var courses = await _courseService.GetAllAsync();

            Assert.IsNotNull(courses);
            Assert.AreNotEqual(0, _courseRepository._courses.Count());

        }


        [TestMethod]
        public async Task GetAllById_WithExistingId_ReturnOneCourse()
        {
            //Arrange 
            var course = new Course
            {
                CourseId = 1,
                Code = "A112A",
                Title = "Introduction to Computer Science",
                Credits = 3,
                Description = "Basic concepts of programming, algorithms, and problem-solving.",
                Instructor = "Dr. Sharma",
                StartDate = new DateTimeOffset(2026, 1, 15, 9, 0, 0, TimeSpan.FromHours(5.75)), // Jan 15, 2026, 9 AM NPT
                EndDate = new DateTimeOffset(2026, 5, 15, 17, 0, 0, TimeSpan.FromHours(5.75)),  // May 15, 2026, 5 PM NPT
                IsActive = true,
                Capacity = 20,
                EnrollmentStartDate = new DateTimeOffset(2025, 12, 25, 8, 0, 0, TimeSpan.FromHours(5.75)), // today
                EnrollmentEndDate = new DateTimeOffset(2026, 1, 10, 23, 59, 0, TimeSpan.FromHours(5.75))   // Jan 10, 2026
            };

            await _courseService.CreateAsync(course);

            //Act 

            var exisitngCourse = await _courseService.GetByIdAsync(course.CourseId);

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

            Assert.IsNull(exisitngCourse);
        }

    }
}
