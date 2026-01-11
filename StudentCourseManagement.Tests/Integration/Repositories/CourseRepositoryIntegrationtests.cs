using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper;
using StudentCourseManagement.Domain.Entities;
using System.Transactions;

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
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);


            var courseId = await CreateCourseAsync();


            Assert.IsNotNull(courseId);
            Assert.IsTrue(courseId > 0);
            Assert.AreNotEqual(0, courseId);
        }

        [TestMethod]
        public async Task GetAllAsync_IfNotNullThrn_ReturnsListOfCourses()
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await CreateCourseAsync();
            var courses = await _repository.GetAllAsync();

            Assert.IsNotNull(courses);
            Assert.IsTrue(courses.Any());
        }

        [TestMethod]
        public async Task GetByIdAsync_WithExistingId_ReturnsCourse()
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            //assume id 1 exists 
            int id = 2;
            var course = await _repository.GetByIdAsync(id);

            Assert.IsNotNull(course);
        }

        [TestMethod]
        public async Task UpdateAsync_WithExistingId_ReturnsTrue()
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var courseId = await CreateCourseAsync();

            var updateCourseData = new Course
            {
                CourseId = courseId,
                Code = "CS1012",
                Title = "Introduction to Programming 2",
                Credits = 3,
                Description = "Fundamentals of programming using C# and .NET Core.",
                Instructor = "Dr. Anil Sharma",
                StartDate = DateTimeOffset.UtcNow.AddDays(40),
                EndDate = DateTimeOffset.UtcNow.AddMonths(2),
                IsActive = true,
                Capacity = 50,
                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(5),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(30),

            };

            var isUpdated = await _repository.UpdateAsync(courseId, updateCourseData);

            Assert.IsTrue(isUpdated);

            var course = await _repository.GetByIdAsync(courseId);
            Assert.AreEqual(updateCourseData.Code, course?.Code);
        }

        [TestMethod]
        public async Task DeleteAsync_WithActiveExistingId_ReturnTrue()
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var courseId = await CreateCourseAsync();


            var isDeleted = await _repository.DeleteAsync(courseId);

            Assert.IsTrue(isDeleted);
            var course = await _repository.GetByIdAsync(courseId);
            Assert.IsNull(course);
        }
        #endregion

        #region Course Validation 

        [TestMethod]
        public async Task CodeExistsAsync_IfCodeExists_ReturnTrue()
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var courseId = await CreateCourseAsync();

            var course = await _repository.GetByIdAsync(courseId);
            var codeExists = await _repository.CodeExistsAsync(course.Code);

            Assert.IsTrue(codeExists);

        }


        [TestMethod]
        public async Task TitleExistsAsync_IfExists_ReturnTrue()
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            string title = "Introduction to Programming";

            var titleExists = await _repository.TitleExistsAsync(title);

            Assert.IsTrue(titleExists);
        }
        #endregion

        #region Enrollment required method 

        [TestMethod]
        public async Task CheckEnrollmentDateAsync_WithEnrollmentDateOut_ReturnsFalse()
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);



            var courseId = await CreateCourseAsync();

            // pick any date outside the enrollment date window 
            var testDate = DateTimeOffset.UtcNow.AddMonths(4);


            var isValid = await _repository.CheckEnrollmentDateAsync(courseId, testDate);

            Assert.IsFalse(isValid);

        }
        #endregion

        #region Private Helper Methods 

        private async Task<int> CreateCourseAsync()
        {
            var course = new Course
            {
                Code = "CS101",
                Title = "Introduction to Programming",
                Credits = 3,
                Description = "Fundamentals of programming using C# and .NET Core.",
                Instructor = "Dr. Anil Sharma",
                StartDate = new DateTimeOffset(2026, 02, 01, 9, 0, 0, TimeSpan.FromHours(5.75)),
                EndDate = new DateTimeOffset(2026, 05, 30, 17, 0, 0, TimeSpan.FromHours(5.75)),
                IsActive = true,
                Capacity = 50,
                EnrollmentStartDate = new DateTimeOffset(2026, 01, 15, 8, 0, 0, TimeSpan.FromHours(5.75)),
                EnrollmentEndDate = new DateTimeOffset(2026, 01, 31, 17, 0, 0, TimeSpan.FromHours(5.75))
            };

            return await _repository.AddAsync(course);
        }
        #endregion
    }
}
