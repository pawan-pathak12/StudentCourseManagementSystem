using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper;
using StudentCourseManagement.Domain.Entities;
using System.Transactions;

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
            //Arrange 
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            // Act
            var studentId = await CreateStudentAsync();

            // Assert
            Assert.IsTrue(studentId > 0);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsListOfStudentData()
        {
            //Arrange 
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            await CreateStudentAsync();
            //Act 

            var result = await _repository.GetAllAsync();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }
        [TestMethod]
        public async Task GetByIdAsync_WithExisintgId_ReturnsStudentRecord()
        {
            //Arrange 
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var studentId = await CreateStudentAsync();

            //Act 

            var student = await _repository.GetByIdAsync(studentId);
            //Assert 

            Assert.IsNotNull(student);
            Assert.AreEqual(studentId, student.StudentId);
            Assert.IsTrue(student.IsActive);
        }

        [TestMethod]
        public async Task UpdateAsync_WithExistingId_ReturnsTrue()
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var studentId = await CreateStudentAsync();
            var updateStudentData = new Student
            {
                StudentId = studentId,
                Name = "pawan",
                IsActive = false,
                Address = " ",
                Email = "emnail",
                Gender = "M",
                DOB = new DateTimeOffset(2012, 10, 10, 0, 0, 0, TimeSpan.FromHours(3.2)),
                Number = 9812431234
            };

            var isUpdated = await _repository.UpdateAsync(updateStudentData);

            Assert.IsTrue(isUpdated);
            var student = await _repository.GetByIdAsync(studentId);
            Assert.AreEqual(updateStudentData.Name, student.Name);
            Assert.AreEqual(updateStudentData.Email, student.Email);
        }

        [TestMethod]
        public async Task DeleteAsync_WithActiveAndExistingId_ReturnsTrue()
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var studentId = await CreateStudentAsync();


            var isDeleted = await _repository.DeleteAsync(studentId);

            Assert.IsTrue(isDeleted);

            var student = await _repository.GetByIdAsync(studentId);
            Assert.IsNull(student);
        }

        [TestMethod]
        public async Task IsStudentActiveAsync_WithExistingStudentID_ReturnTrue()
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var studentId = await CreateStudentAsync();

            var isActive = await _repository.IsStudentActiveAsync(studentId);

            Assert.IsTrue(isActive);
        }

        [TestMethod]
        public async Task EmailExistsAsync_IfEmailExists_Returntrue()
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var studentId = await CreateStudentAsync();

            var student = await _repository.GetByIdAsync(studentId);

            //Act 
            var emailExists = await _repository.EmailExistsAsync(student.Email);

            Assert.IsTrue(emailExists);
        }

        #region Private Helper Methods 

        private async Task<int> CreateStudentAsync()
        {
            var student = new Student
            {
                StudentId = 1,
                Name = "Kiran Sharma",
                Email = "kiran.sharma@example.com",
                DOB = new DateTimeOffset(2004, 05, 12, 0, 0, 0, TimeSpan.FromHours(5.75)), // May 12, 2004
                Number = 9812345678,
                IsActive = true,
                Gender = "Female",
                Address = "Biratnagar, Nepal"
            };
            return await _repository.AddAsync(student);
        }
        #endregion

    }
}
