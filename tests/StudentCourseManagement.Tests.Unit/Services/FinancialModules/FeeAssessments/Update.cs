using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.FeeAssessments
{
    [TestClass]
    public class Update : FeeAssessmentServiceTestBase
    {
        [TestMethod]
        public async Task UpdateAsync_WithValidInput_ReturnTrue()
        {
            //Arrange 
            var feeAssessment = new FeeAssessment
            {
                CourseId = 1,
                FeeTemplateId = 1,
                EnrollmentId = 1,
                Amount = 1000,
                IsActive = true,
                FeeAssessmentStatus = AssessmentStatus.Pending
            };
            var feeAssessentId = await _assessmentRepository.AddAsync(feeAssessment);

            var updateFeeAssessment = new FeeAssessment
            {
                FeeAssessmentId = feeAssessentId,
                CourseId = 1,
                FeeTemplateId = 1,
                EnrollmentId = 1,
                Amount = 1000,
                IsActive = true,
                FeeAssessmentStatus = AssessmentStatus.Paid,
                DueDate = DateTimeOffset.UtcNow.AddDays(30)
            };

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
            var updateFeeAssessment = new FeeAssessment
            {
                CourseId = 1,
                FeeTemplateId = 1,
                EnrollmentId = 1,
                Amount = 1000,
                IsActive = true,
                FeeAssessmentStatus = AssessmentStatus.Paid,
                DueDate = DateTimeOffset.UtcNow.AddDays(30)
            };

            //Act 
            var result = await _feeAssessmentService.UpdateAsync(1, updateFeeAssessment);

            //Assert
            Assert.IsFalse(result);
        }

    }
}
