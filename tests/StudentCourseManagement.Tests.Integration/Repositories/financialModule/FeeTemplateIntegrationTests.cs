using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper;
using StudentCourseManagement.Data.Repositories.Dapper.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Common.Builders;
using StudentCourseManagement.Tests.Common.Builders.FinancialModule;
using System.Transactions;

namespace StudentCourseManagement.Tests.Integration.Repositories.financialModule
{
    [TestClass]
    [DoNotParallelize]
    public class FeeTemplateIntegrationTests
    {
        private readonly FeeTemplateRepository _feeTemplateRepository;
        private readonly CourseRepository _courseRepository;
        private TransactionScope _scope;

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
            var courseId = await CreateCourseAsync();

            //Act
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);


            Assert.IsNotNull(feeTemplateId);
            Assert.IsGreaterThan(0, feeTemplateId);

        }

        [TestMethod]
        public async Task GettAllAsync_IfNotNullThen_ReturnListOfFeeTemplate()
        {
            // Arrange
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
            var courseId = await CreateCourseAsync();

            //Act
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);

            var updateData = new FeeTemplateBuilder()
                .WithFeeTemplateId(feeTemplateId).WithCourseId(courseId).WithAmount(45000)
                .Build();


            // Act
            var isUpdated = await _feeTemplateRepository.UpdateAsync(feeTemplateId, updateData);

            // Assert
            Assert.IsTrue(isUpdated);

            var updatedTemplate = await _feeTemplateRepository.GetByIdAsync(feeTemplateId);
            Assert.IsNotNull(updatedTemplate);
            Assert.AreEqual(updateData.Name, updatedTemplate.Name);
            Assert.AreEqual(updateData.Amount, updatedTemplate.Amount);
        }


        [TestMethod]
        public async Task DeleteAsync_WithActiveId_SetsIsActiveFalse()
        {
            // Arrange
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

        #region Phase 3 required method
        [TestMethod]
        public async Task GetActiveByCourseld_ExistingCourse_ReturnsFeeTemplate()
        {
            //Arrange 
            var courseId = await CreateCourseAsync();
            var templateId = await CreateFeeTemplateAsync(courseId);

            //act 
            var result = await _feeTemplateRepository.GetActiveByCourseId(courseId);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<FeeTemplate>(result);
        }
        #endregion

        #region Private Helper Methods 
        private async Task<int> CreateCourseAsync()
        {
            var course = new CourseBuilder().Build();

            return await _courseRepository.AddAsync(course);
        }
        private async Task<int> CreateFeeTemplateAsync(int courseId)
        {
            var feeTemplate = new FeeTemplateBuilder()
                .WithCourseId(courseId).WithCalculationType(CalculationType.RatePerCredit)
                .Build();
            return await _feeTemplateRepository.AddAsync(feeTemplate);
        }
        #endregion
    }
}
