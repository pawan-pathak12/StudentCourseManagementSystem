using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Common;

namespace StudentCourseManagement.Tests.Services.Courses
{
    [TestClass]
    public class CourseServiceTest_Update : CourseServiceTestBase
    {
        [TestMethod]
        public async Task Update_WithExistingID_ReturnTrue()
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

            var course2 = new Course
            {
                CourseId = 1,
                Code = "B203B",
                Title = "Database Systems",
                Credits = 4,
                Description = "Covers relational databases, SQL, normalization, and indexing strategies.",
                Instructor = "Prof. Anjali Karki",
                StartDate = new DateTimeOffset(2026, 2, 1, 10, 0, 0, TimeSpan.FromHours(5.75)), // Feb 1, 2026, 10 AM NPT
                EndDate = new DateTimeOffset(2026, 6, 1, 16, 0, 0, TimeSpan.FromHours(5.75)),   // Jun 1, 2026, 4 PM NPT
                IsActive = true,
                Capacity = 30,
                EnrollmentStartDate = new DateTimeOffset(2026, 1, 10, 9, 0, 0, TimeSpan.FromHours(5.75)), // Jan 10, 2026
                EnrollmentEndDate = new DateTimeOffset(2026, 1, 31, 23, 59, 0, TimeSpan.FromHours(5.75))  // Jan 31, 2026
            };


            //Act 
            var isUpdated = await _courseService.UpdateAsync(course.CourseId, course2);

            Assert.IsTrue(isUpdated);
        }


        [TestMethod]
        public async Task Update_WithNonExistingID_ReturnFalse()
        {
            //Arrange 
            //assume id 111 dont exists

            int id = 111;
            var course2 = new Course
            {
                CourseId = 2,
                Code = "B203B",
                Title = "Database Systems",
                Credits = 4,
                Description = "Covers relational databases, SQL, normalization, and indexing strategies.",
                Instructor = "Prof. Anjali Karki",
                StartDate = new DateTimeOffset(2026, 2, 1, 10, 0, 0, TimeSpan.FromHours(5.75)), // Feb 1, 2026, 10 AM NPT
                EndDate = new DateTimeOffset(2026, 6, 1, 16, 0, 0, TimeSpan.FromHours(5.75)),   // Jun 1, 2026, 4 PM NPT
                IsActive = true,
                Capacity = 30,
                EnrollmentStartDate = new DateTimeOffset(2026, 1, 10, 9, 0, 0, TimeSpan.FromHours(5.75)), // Jan 10, 2026
                EnrollmentEndDate = new DateTimeOffset(2026, 1, 31, 23, 59, 0, TimeSpan.FromHours(5.75))  // Jan 31, 2026
            };


            //Act 
            var isUpdated = await _courseService.UpdateAsync(id, course2);

            Assert.IsFalse(isUpdated);
        }

    }
}
