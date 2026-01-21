using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

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
            var feeTemplate = new FeeTemplate
            {
                FeeTemplateId = 0,
                CourseId = courseId,
                Amount = 50000,
                IsActive = true
            };
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
            var feeTemplate = new FeeTemplate
            {
                CreatedAt = DateTimeOffset.UtcNow,
                Amount = 1000,
                CalculationType = CalculationType.FlatAmount,
                IsActive = true
            };

            //Act
            var (sucess, errorMessage, feeTemplateId) = await _feeTemplateService.CreateAsync(feeTemplate);

            //Assert 
            Assert.IsFalse(sucess);

        }

        #region Helper Method

        private async Task<int> CreateCourse()
        {
            var course = new Course
            {
                Title = "C#",
                IsActive = true
            };
            return await _courseRepository.AddAsync(course);

        }


        #endregion
    }
}
