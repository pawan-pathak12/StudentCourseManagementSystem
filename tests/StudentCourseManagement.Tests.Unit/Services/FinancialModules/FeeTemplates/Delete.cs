using StudentCourseManagement.Tests.Common.Builders;
using StudentCourseManagement.Tests.Common.Builders.FinancialModule;
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
            var course = new CourseBuilder().Build();
            var courseId = await _courseRepository.AddAsync(course);

            var feeTemplate = new FeeTemplateBuilder()
               .WithCourseId(courseId).WithAmount(4000).Build();
            var feetemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);

            //Act 
            var result = await _feeTemplateService.DeleteAsync(feetemplateId);

            //Assert 
            Assert.IsTrue(result);

            var feeTemplateData = await _feeTemplateRepository.GetByIdAsync(feetemplateId);

            Assert.IsNull(feeTemplateData);

        }

    }
}
