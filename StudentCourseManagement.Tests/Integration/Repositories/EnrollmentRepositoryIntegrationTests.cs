using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Tests.Integration.Repositories
{
    [TestClass]
    public class EnrollmentRepositoryIntegrationTests
    {
        private readonly EnrollmentRepository _repository;
        public EnrollmentRepositoryIntegrationTests()
        {
            var fixture = new DatabaseFixture();
            var loggerMock = new Mock<ILogger<EnrollmentRepository>>();
            _repository = new EnrollmentRepository(fixture.DbContext, loggerMock.Object);
        }

        [TestMethod]
        public async Task AddAsync_WithValidData_InsertData()
        {
            var enrollment = new Enrollment
            {
                StudentId = 1,
                CourseId = 2
            };

            var enrollmentId = await _repository.AddAsync(enrollment);

            Assert.AreNotEqual(0, enrollmentId);
            Assert.IsNotNull(enrollmentId);
        }

        [TestMethod]
        public async Task GetAll_IfNotNull_ReturnEnrollments()
        {
            var enrollment = await _repository.GetAllAsync();

            Assert.IsNotNull(enrollment);
        }
        [TestMethod]
        public async Task GetAll_WithExistingData_ReturnEnrollment()
        {
            // assume enrollment id 2 exists
            int id = 2;

            var enrollment = await _repository.GetByIdAsync(id);
            Assert.IsNotNull(enrollment);
        }
        [TestMethod]
        public async Task Update_WithExistingId_ReturnTrue()
        {
            int id = 2;
            var enrollment = new Enrollment
            {
                EnrollmentId = id,
                StudentId = 1,
                CourseId = 2,
                IsActive = true
            };

            var isUpdated = await _repository.UpdateAsync(id, enrollment);

            Assert.IsTrue(isUpdated);
        }
        [TestMethod]
        public async Task DeleteAsync_WithExistingData_ReturnsFalse()
        {
            var id = 2;
            var isDeleted = await _repository.DeleteAsync(id);

            Assert.IsTrue(isDeleted);
        }

    }
}
