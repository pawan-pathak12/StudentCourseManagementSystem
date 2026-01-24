using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.FeeTemplates
{
    [TestClass]
    public class Delete : FeeTemplateServiceBase
    {
        [TestMethod]
        public async Task DeleteAsync_WithExistingId_ReturnTrue()
        {
            //Arrange 
            var feetemplate = new FeeTemplate
            {
                CourseId = 1,
                CreatedAt = DateTimeOffset.UtcNow,
                IsActive = true
            };

            var feetemplateId = await _feeTemplateRepository.AddAsync(feetemplate);
            //Act 
            var result = await _feeTemplateService.DeleteAsync(feetemplateId);

            //Assert 
            Assert.IsTrue(result);

            var feeTemplateData = await _feeTemplateRepository.GetByIdAsync(feetemplateId);

            Assert.IsNull(feeTemplateData);

        }

    }
}
