using StudentCourseManagement.Domain.Constants;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.FeeAssessments
{
    [TestClass]
    public class Update : FeeAssessmentServiceTestBase
    {
        [TestMethod]
        public async Task UpdateAsync_WithValidInput_ReturnTrue()
        {
            //Arrange 
            var feeAssessmentData = new FeeAssessmentBuilder()
                .WithCourseId(1).WithFeeTemplateId(1).WithEnrollmentId(1)
                .WithIsActive(true).WithAmount(100).WithFeeAssessmentStatus(AssessmentStatus.Pending).Build();


            var feeAssessentId = await _assessmentRepository.AddAsync(feeAssessmentData);

            var updateFeeAssessment = new FeeAssessmentBuilder()
              .WithCourseId(1).WithFeeTemplateId(1).WithEnrollmentId(1).WithFeeAssessmentId(feeAssessentId)
              .WithIsActive(true).WithAmount(100).WithFeeAssessmentStatus(AssessmentStatus.Paid)
              .WithDueDate(DateTimeOffset.UtcNow.AddDays(FinancialConstants.DUE_DATE_DAYS)).Build();


            //Act 
            var result = await _feeAssessmentService.UpdateAsync(feeAssessentId, updateFeeAssessment);

            //Assert
            Assert.IsTrue(result);

            var assessment = await _assessmentRepository.GetByIdAsync(feeAssessentId);
            Assert.AreEqual(updateFeeAssessment.DueDate, assessment.DueDate);
            Assert.AreEqual(updateFeeAssessment.FeeAssessmentStatus, assessment.FeeAssessmentStatus);
        }

        [TestMethod]

        public async Task UpdateAsync_WithNonExistingId_Returnfalse()
        {
            var updateFeeAssessment = new FeeAssessmentBuilder()
              .WithCourseId(1).WithFeeTemplateId(1).WithEnrollmentId(1)
              .WithIsActive(true).WithAmount(100).WithFeeAssessmentStatus(AssessmentStatus.Pending).Build();

            //Act 
            var result = await _feeAssessmentService.UpdateAsync(1, updateFeeAssessment);

            //Assert
            Assert.IsFalse(result);
        }

    }
}
