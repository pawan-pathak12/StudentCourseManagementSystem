using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.FeeAssessments
{
    [TestClass]
    public class Delete : FeeAssessmentServiceTestBase
    {
        [TestMethod]
        public async Task DeleteAsync_WithExistingFeeAssessemnt_ReturnsTrue()
        {
            //Arrange 
            var feeAssessmentData = new FeeAssessment
            {
                CourseId = 1,
                FeeTemplateId = 1,
                EnrollmentId = 1,
                IsActive = true,
                Amount = 100
            };
            var feeAssessmentId = await _assessmentRepository.AddAsync(feeAssessmentData);

            //Act 
            var result = await _feeAssessmentService.DeleteAsync(feeAssessmentId);

            //Assert 
            Assert.IsTrue(result);

            var feeAssessment = await _feeAssessmentService.GetByIdAsync(feeAssessmentId);
            Assert.IsNull(feeAssessment);

        }
    }
}
