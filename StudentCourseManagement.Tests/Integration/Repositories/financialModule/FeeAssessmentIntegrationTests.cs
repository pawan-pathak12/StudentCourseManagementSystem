using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Tests.Integration.Repositories.financialModule
{
    [TestClass]
    public class FeeAssessmentIntegrationTests
    {
        private readonly FeeAssessmentRepository _feeAssessment;
        public FeeAssessmentIntegrationTests()
        {
            var fixture = new DatabaseFixture();
            var loggerMock = new Mock<ILogger<FeeAssessmentRepository>>();

            _feeAssessment = new FeeAssessmentRepository(fixture.DbContext, loggerMock.Object);
        }

        #region CURD Operations 

        [TestMethod]
        public async Task AddAsync_WithValidData_InsertData()
        {
            var feeAssessment = new FeeAssessment
            {
                EnrollmentId = 2,
                CourseId = 2,
                FeeTemplateId = 0,
                Amount = 15000.00m,
                DueDate = new DateTimeOffset(2026, 01, 20, 17, 0, 0, TimeSpan.FromHours(5.75)),
                FeeAssessmentStatus = AssessmentStatus.Pending,
                IsActive = true,
                PaidDate = null,
                LateFeeAmount = null,
                LateFeeAppliedDate = null
            };

            var feeAssessmentId = await _feeAssessment.AddAsync(feeAssessment);

            Assert.IsNotNull(feeAssessmentId);
            Assert.AreNotEqual(0, feeAssessmentId);
        }

        [TestMethod]
        public async Task GettAllAsync_IfNotNullThen_ReturnListOfFeeAssessment()
        {
            var feeAssessments = await _feeAssessment.GetAllAsync();

            Assert.IsNotNull(feeAssessments);
            Assert.AreNotEqual(0, feeAssessments.Count());
        }

        [TestMethod]
        public async Task GetByIdAsync_IfNotNull_ReturnFeeAssessment()
        {
            //assume feeAssessmentId =1 exists
            int id = 1;
            var feeAssesment = await _feeAssessment.GetByIdAsync(id);

            Assert.IsNotNull(feeAssesment);
        }
        [TestMethod]
        public async Task UpdateAsync_WithValidInsert_ReturnTrue()
        {
            /* requirement to pass this test : 
           enrollment id =2 must exists
          courseid =2 must exists , 
          invoice id =1 , feetemplate =1 
          */
            var feeAssessment = new FeeAssessment
            {
                EnrollmentId = 2,
                CourseId = 2,
                FeeTemplateId = 1,
                Amount = 15000.00m,
                DueDate = new DateTimeOffset(2026, 01, 20, 17, 0, 0, TimeSpan.FromHours(5.75)),
                FeeAssessmentStatus = AssessmentStatus.Pending,
                IsActive = true,
                PaidDate = null,
                LateFeeAmount = null,
                LateFeeAppliedDate = null
            };

            var feeAssementId = await _feeAssessment.AddAsync(feeAssessment);

            var feeAssement2 = new FeeAssessment
            {
                FeeAssessmentStatus = AssessmentStatus.Generated
            };

            var isUpdated = await _feeAssessment.UpdateAsync(feeAssementId, feeAssement2);

            Assert.IsTrue(isUpdated);

        }

        [TestMethod]
        public async Task DeleteAsync_WithExistingActiveId_ReturnsTrue()
        {
            /* requirement to pass this test : 
             enrollment id =2 must exists
            courseid =2 must exists , 
            invoice id =1 , feetemplate =1 
            */
            var feeAssessment = new FeeAssessment
            {
                EnrollmentId = 2,
                CourseId = 2,
                FeeTemplateId = 1,
                Amount = 15000.00m,
                DueDate = new DateTimeOffset(2026, 01, 20, 17, 0, 0, TimeSpan.FromHours(5.75)),
                FeeAssessmentStatus = AssessmentStatus.Pending,
                IsActive = true,
                PaidDate = null,
                LateFeeAmount = null,
                LateFeeAppliedDate = null
            };

            var feeAssementId = await _feeAssessment.AddAsync(feeAssessment);

            var isDeleted = await _feeAssessment.DeleteAsync(feeAssementId);

            Assert.IsTrue(isDeleted);

        }

        #endregion
    }
}
