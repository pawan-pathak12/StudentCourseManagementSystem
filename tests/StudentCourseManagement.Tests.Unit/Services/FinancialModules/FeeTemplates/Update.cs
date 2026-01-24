using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.FeeTemplates
{
    [TestClass]
    public class Update : FeeTemplateServiceBase
    {
        [TestMethod]
        public async Task UpdateAsync_WithValidData_ReturnTrue()
        {
            //Arrange 
            var course = new Course
            {
                Title = "C#",
                IsActive = true
            };
            var courseId = await _courseRepository.AddAsync(course);

            var feeTemplate = new FeeTemplate
            {
                CourseId = courseId,
                CreatedAt = DateTimeOffset.UtcNow,
                IsActive = true,
                Name = "Lab fee"
            };

            var feeTemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);

            var updateFeeTemplate = new FeeTemplate
            {
                CourseId = courseId,
                FeeTemplateId = feeTemplateId,
                Amount = 1000,
                Name = "Tution fee"
            };

            //Act 
            var result = await _feeTemplateService.UpdateAsync(feeTemplateId, updateFeeTemplate);

            //Assert 
            Assert.IsTrue(result);

            var feeTemplateData = await _feeTemplateRepository.GetByIdAsync(feeTemplateId);
            Assert.AreEqual(updateFeeTemplate.Amount, feeTemplateData.Amount);
            Assert.AreEqual(updateFeeTemplate.Name, feeTemplateData.Name);
        }

        [TestMethod]
        public async Task UpdateAsync_WithNonExistingCourse_ReturnFalse()
        {
            //Arrange 
            var feeTemplate = new FeeTemplate
            {
                CreatedAt = DateTimeOffset.UtcNow,
                IsActive = true,
                Name = "Lab fee"
            };

            var feeTemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);

            var updateFeeTemplate = new FeeTemplate
            {
                FeeTemplateId = feeTemplateId,
                Amount = 1000,
                Name = "Tution fee"
            };

            //Act 
            var result = await _feeTemplateService.UpdateAsync(feeTemplateId, updateFeeTemplate);

            //Assert 
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task UpdateAsync_WithNonExistingFeeTemplate_ReturnFalse()
        {
            //Arrange 
            var course = new Course
            {
                Title = "C#",
                IsActive = true
            };
            var courseId = await _courseRepository.AddAsync(course);

            var feeTemplateId = 11;

            var updateFeeTemplate = new FeeTemplate
            {
                FeeTemplateId = feeTemplateId,
                CourseId = courseId,
                Amount = 1000,
                Name = "Tution fee"
            };

            //Act 
            var result = await _feeTemplateService.UpdateAsync(feeTemplateId, updateFeeTemplate);

            //Assert 
            Assert.IsFalse(result);
        }
    }
}
