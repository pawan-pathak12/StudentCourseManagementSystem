using StudentCourseManagement.Tests.Unit.Common.FInacialModules;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.FeeAssessments
{
    [TestClass]
    public class Delete : FeeAssessmentServiceTestBase
    {
        [TestMethod]
        public async Task DeleteAsync_WithExistingFeeAssessemnt_ReturnsTrue()
        {
            //Arrange 
            var feeAssessmentData = new FeeAssessmentBuilder()
                .WithCourseId(1).WithFeeTemplateId(1).WithEnrollmentId(1)
                .WithIsActive(true).WithAmount(100).Build();

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
