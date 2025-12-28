using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Tests.Integration.Repositories
{
    [TestClass]
    public class CourseRepositoryIntegrationtests
    {
        private readonly CourseRepository _repository;
        public CourseRepositoryIntegrationtests()
        {
            var fixture = new DatabaseFixture();
            var loggerMock = new Mock<ILogger<CourseRepository>>();
            _repository = new CourseRepository(fixture.DbContext, loggerMock.Object);
        }
        #region CURD Operations 

        [TestMethod]
        public async Task AddAsync_WithValidCourse_InsertData()
        {
            var course = new Course
            {
                Code = "CS101",
                Title = "Introduction to Programming",
                Credits = 3,
                Description = "Learn the basics of programming using C# and .NET.",
                Instructor = "Dr. Anil Kumar",
                StartDate = new DateTimeOffset(2025, 1, 15, 9, 0, 0, TimeSpan.Zero),
                EndDate = new DateTimeOffset(2025, 5, 15, 17, 0, 0, TimeSpan.Zero),
                Capacity = 50,
                EnrollmentStartDate = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero),
                EnrollmentEndDate = new DateTimeOffset(2025, 1, 10, 23, 59, 59, TimeSpan.Zero)
            };

            var courseId = await _repository.AddAsync(course);

            Assert.IsNotNull(courseId);
            Assert.AreNotEqual(0, courseId);
        }

        [TestMethod]
        public async Task GetAllAsync_IfNotNullThrn_ReturnsListOfCourses()
        {
            var courses = await _repository.GetAllAsync();

            Assert.IsNotNull(courses);
        }

        [TestMethod]
        public async Task GetByIdAsync_WithExistingId_ReturnsCourse()
        {
            //assume id 1 exists 
            int id = 2;
            var course = await _repository.GetByIdAsync(id);

            Assert.IsNotNull(course);
        }

        [TestMethod]
        public async Task UpdateAsync_WithExistingId_ReturnsTrue()
        {
            var id = 2;

            var course = new Course
            {
                CourseId = id,
                Code = "tesitng 2",
                Title = "Introduction to Programming",
                Credits = 3,
                IsActive = true

            };
            var isUpdated = await _repository.UpdateAsync(id, course);

            Assert.IsTrue(isUpdated);
        }

        [TestMethod]
        public async Task DeleteAsync_WithActiveExistingId_ReturnTrue()
        {
            // assme id 1 exists 

            int id = 2;
            var isDeleted = await _repository.DeleteAsync(id);

            Assert.IsTrue(isDeleted);
        }
        #endregion

        #region Course Validation 

        [TestMethod]
        public async Task CodeExistsAsync_IfCodeExists_ReturnTrue()
        {
            string code = "tesitng 2";

            var codeExists = await _repository.CodeExistsAsync(code);

            Assert.IsTrue(codeExists);

        }


        [TestMethod]
        public async Task TitleExistsAsync_IfExists_ReturnTrue()
        {
            string title = "Introduction to Programming";

            var titleExists = await _repository.TitleExistsAsync(title);

            Assert.IsTrue(titleExists);
        }
        #endregion
    }
}
