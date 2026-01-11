using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper;
using StudentCourseManagement.Data.Repositories.Dapper.FinancialModule;
using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using System.Transactions;

namespace StudentCourseManagement.Tests.Integration.Repositories.financialModule
{
    [TestClass]
    public class FeeTemplateIntegrationTests
    {
        private readonly FeeTemplateRepository _feeTemplateRepository;
        private readonly CourseRepository _courseRepository;

        public FeeTemplateIntegrationTests()
        {
            var databasefixture = new DatabaseFixture();
            var mocklogger = new Mock<ILogger<FeeTemplateRepository>>();
            var mockLoggerCourse = new Mock<ILogger<CourseRepository>>();
            _courseRepository = new CourseRepository(databasefixture.DbContext, mockLoggerCourse.Object);
            _feeTemplateRepository = new FeeTemplateRepository(databasefixture.DbContext, mocklogger.Object);
        }

        #region CURD Operations 
        [TestMethod]
        public async Task AddAsync_WithValidFeeTemplate_InsertsRowAndReturnsId()
        {
            // Arrange
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var courseId = await CreateCourseAsync();

            //Act
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);


            Assert.IsNotNull(feeTemplateId);
            Assert.IsTrue(feeTemplateId > 0);

        }

        [TestMethod]
        public async Task GettAllAsync_IfNotNullThen_ReturnListOfFeeTemplate()
        {
            // Arrange
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var courseId = await CreateCourseAsync();
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);


            //Act 
            var feetemplates = await _feeTemplateRepository.GetAllAsync();

            //Assert 
            Assert.IsTrue(feetemplates.Any());
            Assert.AreNotEqual(0, feetemplates.Count());

        }

        [TestMethod]
        public async Task GetByIdAsync_WithExistingId_ReturnsFeeTemplate()
        {
            // Arrange
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var courseId = await CreateCourseAsync();
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);

            //act 
            var feeTempalateData = await _feeTemplateRepository.GetByIdAsync(feeTemplateId);


            Assert.IsNotNull(feeTempalateData);

        }

        [TestMethod]
        public async Task UpdateAsync_WithExistingId_ReturnsTrue_AndUpdatesData()
        {
            // Arrange
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var courseId = await CreateCourseAsync();

            //Act
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);


            var updateData = new FeeTemplate
            {
                FeeTemplateId = feeTemplateId,
                Name = "Updated Fee Template",
                Amount = 45000m,
                UpdatedAt = DateTimeOffset.UtcNow,
                IsActive = true,
                CourseId = courseId
            };

            // Act
            var isUpdated = await _feeTemplateRepository.UpdateAsync(feeTemplateId, updateData);

            // Assert
            Assert.IsTrue(isUpdated);

            var updatedTemplate = await _feeTemplateRepository.GetByIdAsync(feeTemplateId);
            Assert.IsNotNull(updatedTemplate);
            Assert.AreEqual("Updated Fee Template", updatedTemplate.Name);
            Assert.AreEqual(45000m, updatedTemplate.Amount);
        }


        [TestMethod]
        public async Task DeleteAsync_WithActiveId_SetsIsActiveFalse()
        {
            // Arrange
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var courseId = await CreateCourseAsync();
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);

            //Act
            var isdeleted = await _feeTemplateRepository.DeleteAsync(feeTemplateId);

            //Assert 
            Assert.AreEqual(true, isdeleted);
            var feeTemplate = await _feeTemplateRepository.GetByIdAsync(feeTemplateId);
            Assert.IsNull(feeTemplate);

        }
        #endregion

        #region Private Helper Methods 
        private async Task<int> CreateCourseAsync()
        {
            var course = new Course
            {
                Code = "CS1001",
                Title = "Introduction to Programming",
                Credits = 3,
                Description = "Fundamentals of programming using C# and .NET Core.",
                Instructor = "Dr. Anil Sharma",
                StartDate = DateTimeOffset.UtcNow.AddDays(40),
                EndDate = DateTimeOffset.UtcNow.AddMonths(2),
                IsActive = true,
                Capacity = 50,
                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(10),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(25),
            };

            return await _courseRepository.AddAsync(course);
        }
        private async Task<int> CreateFeeTemplateAsync(int courseId)
        {
            var feeTemplate = new FeeTemplate
            {
                Name = "Undergraduate Tuition Template",
                CourseId = courseId,
                CalculationType = CalculationType.RatePerCredit,
                RatePerCredit = 2500.00m,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = null,
                Amount = 0
            };
            return await _feeTemplateRepository.AddAsync(feeTemplate);
        }
        #endregion
    }
}
