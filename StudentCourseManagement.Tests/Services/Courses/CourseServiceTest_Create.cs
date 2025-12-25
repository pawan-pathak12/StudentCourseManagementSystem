using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Common;

namespace StudentCourseManagement.Tests.Services.Courses
{
    [TestClass]
    public class CourseServiceTest_Create : CourseServiceTestBase
    {
        [TestMethod]
        public async Task CreateCourse_ReturnCreatedCourseId()
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

            //Act 
            var createdId = await _courseService.Create(course);

            Assert.AreEqual(1, _courseRepository._courses.Count());
            Assert.AreEqual(createdId, course.CourseId);

        }
    }
}
