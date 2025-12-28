using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Tests.Integration.Repositories
{
    [TestClass]
    public class StudentRepositoryIntegrationTests
    {
        private readonly StudentRepository _repository;
        public StudentRepositoryIntegrationTests()
        {
            var fixture = new DatabaseFixture();
            var loggerMock = new Mock<ILogger<StudentRepository>>();
            _repository = new StudentRepository(fixture.DbContext, loggerMock.Object);
        }
        [TestMethod]
        public async Task Create_WithValidStudent_InsertRecord()
        {
            // Arrange
            var student = new Student
            {
                Name = "Ram",
                Email = "ram.test@test.com",
                IsActive = true
            };

            // Act
            var result = await _repository.AddAsync(student);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsListOfStudentData()
        {
            var result = await _repository.GetAllAsync();

            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task GetByIdAsync_WithExisintgId_ReturnsStudentRecord()
        {
            //let assmune student id 1 exists 
            int id = 1;
            var student = await _repository.GetByIdAsync(id);

            Assert.IsNotNull(student);
            Assert.AreEqual(id, student.StudentId);
            Assert.IsTrue(student.IsActive);
        }
        [TestMethod]
        public async Task GetByIdAsync_WithNonExisintgId_ReturnNull()
        {
            //let assume student id 555 dont exists 
            int id = 555;
            var student = await _repository.GetByIdAsync(id);

            Assert.IsNull(student);
        }
        [TestMethod]

        public async Task UpdateAsync_WithExistingId_ReturnsTrue()
        {
            var id = 1;
            var student = new Student
            {
                StudentId = id,
                Name = "pawan",
                IsActive = false,
                Address = " ",
                Email = "emnail",
                Gender = "M"
            };

            var isUpdated = await _repository.UpdateAsync(student);

            Assert.IsTrue(isUpdated);
        }

        [TestMethod]
        public async Task DeleteAsync_WithActiveAndExistingId_ReturnsTrue()
        {
            int id = 1;

            var isDeleted = await _repository.DeleteAsync(id);
            Assert.IsTrue(isDeleted);
        }

    }
}
