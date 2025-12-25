using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Common;

namespace StudentCourseManagement.Tests.Services.Courses
{
    [TestClass]
    public class CourseServiceTest_Delete : CourseServiceTestBase
    {
        [TestMethod]
        public async Task Delete_WithExistingId_ReturnTrue()
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

            await _courseService.Create(course);

            //Act 
            var isDeleted = await _courseService.Delete(course.CourseId);

            Assert.IsTrue(isDeleted);
        }


        [TestMethod]
        public async Task Delete_WithNonExistingId_ReturnFalse()
        {
            //Arrange 
            //Assume Courseid 1234 dont exists 
            int id = 1234;


            //Act 
            var isDeleted = await _courseService.Delete(id);


            Assert.IsFalse(isDeleted);
        }
    }
}
