using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper;
using StudentCourseManagement.Tests.Common.Builders;
using System.Transactions;

namespace StudentCourseManagement.Tests.Integration.Repositories
{
    [TestClass]
    [DoNotParallelize]
    public class StudentRepositoryIntegrationTests
    {
        private readonly StudentRepository _repository;
        private TransactionScope _scope;
        public StudentRepositoryIntegrationTests()
        {
            var fixture = new DatabaseFixture();
            var loggerMock = new Mock<ILogger<StudentRepository>>();
            _repository = new StudentRepository(fixture.DbContext, loggerMock.Object);
        }

        [TestInitialize]
        public void Init()
        {
            _scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _scope.Dispose(); // rollback
        }
        [TestMethod]
        public async Task Create_WithValidStudent_InsertRecord()
        {
            //Arrange 

            // Act
            var studentId = await CreateStudentAsync();

            // Asser
            Assert.IsGreaterThan(0, studentId);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsListOfStudentData()
        {
            //Arrange 

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
            //Arrange 
            var studentId = await CreateStudentAsync();

            var updateStudentData = new StudentBuilder()
                .WithName("Pawan").WithEmail("pawan@gmail.com")
                .WithId(studentId).Build();

            //Act
            var isUpdated = await _repository.UpdateAsync(updateStudentData);

            //Assert 

            Assert.IsTrue(isUpdated);
            var student = await _repository.GetByIdAsync(studentId);
            Assert.AreEqual(updateStudentData.Name, student?.Name);
            Assert.AreEqual(updateStudentData.Email, student?.Email);
        }

        [TestMethod]
        public async Task DeleteAsync_WithActiveAndExistingId_ReturnsTrue()
        {
            //Arange 
            var studentId = await CreateStudentAsync();

            //act 
            var isDeleted = await _repository.DeleteAsync(studentId);

            //Assert
            Assert.IsTrue(isDeleted);
            var student = await _repository.GetByIdAsync(studentId);
            Assert.IsNull(student);
        }

        [TestMethod]
        public async Task IsStudentActiveAsync_WithExistingStudentID_ReturnTrue()
        {
            //Arrange 
            var studentId = await CreateStudentAsync();

            var isActive = await _repository.IsStudentActiveAsync(studentId);
            //Assert
            Assert.IsTrue(isActive);
        }

        [TestMethod]
        public async Task EmailExistsAsync_IfEmailExists_Returntrue()
        {
            //Arrange 
            var studentId = await CreateStudentAsync();

            var student = await _repository.GetByIdAsync(studentId);

            //Act 
            var emailExists = await _repository.EmailExistsAsync(student.Email);

            Assert.IsTrue(emailExists);
        }

        #region Private Helper Methods 

        private async Task<int> CreateStudentAsync()
        {
            var student = new StudentBuilder()
                .Build();

            return await _repository.AddAsync(student);
        }
        #endregion

    }
}
