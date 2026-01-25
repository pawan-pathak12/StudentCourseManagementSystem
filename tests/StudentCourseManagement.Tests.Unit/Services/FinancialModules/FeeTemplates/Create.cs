using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.FeeTemplates
{
    [TestClass]
    public class Create : FeeTemplateServiceBase
    {
        [TestMethod]
        public async Task CreateAsync_WithValidData_ReturnsTrue()
        {
            //Arrange 
            var courseId = await CreateCourse();

            //feetemplate Id = feetemplate+1 ; 
            var feeTemplate = new FeeTemplateBuilder()
                .WithCourseId(courseId).WithAmount(4000).Build();

            //Act 
            var (sucess, errorMessage, feeTemplateId) = await _feeTemplateService.CreateAsync(feeTemplate);

            //Assert 
            Assert.IsTrue(sucess);

            var feeTemplateData = await _feeTemplateRepository.GetByIdAsync(1);
            Assert.IsNotNull(feeTemplateData);

        }
        [TestMethod]
        public async Task CreateAsync_WhenCourseNotFound_Returnfalse()
        {
            //Arrange 
            var feeTemplate = new FeeTemplateBuilder()
                .WithCalculationType(CalculationType.FlatAmount)
               .WithAmount(4000).Build();

            //Act
            var (sucess, errorMessage, feeTemplateId) = await _feeTemplateService.CreateAsync(feeTemplate);

            //Assert 
            Assert.IsFalse(sucess);

        }

        #region Helper Method

        private async Task<int> CreateCourse()
        {
            var course = new CourseBuilder()
                .WithTitle("C# master class").Build();
            return await _courseRepository.AddAsync(course);
        }


        #endregion
    }
}
