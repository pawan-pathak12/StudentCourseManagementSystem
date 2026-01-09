using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper;
using StudentCourseManagement.Data.Repositories.Dapper.FinancialModule;
using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Tests.Integration.Repositories.financialModule
{
    [TestClass]
    public class FeeTemplateIntegrationTests
    {
        private readonly FeeTemplateRepository _feeTemplateRepository;
        private readonly CourseRepository _courseRepository;

        public FeeTemplateIntegrationTests()
        {
            var databasefixture = new DatabaseFixture();
            var mocklogger = new Mock<ILogger<FeeTemplateRepository>>();
            var mockLoggerCourse = new Mock<ILogger<CourseRepository>>();
            _courseRepository = new CourseRepository(databasefixture.DbContext, mockLoggerCourse.Object);
            _feeTemplateRepository = new FeeTemplateRepository(databasefixture.DbContext, mocklogger.Object);
        }

        #region CURD Operations 
        [TestMethod]
        public async Task AddAsync_WithValidFeeTemplate_InsertsRowAndReturnsId()
        {
            #region Arrange
            var course = new Course
            {
                Code = "CS101",
                Title = "Introduction to Programming",
                Credits = 3,
                Description = "Fundamentals of programming using C# and .NET Core.",
                Instructor = "Dr. Anil Sharma",
                StartDate = DateTimeOffset.UtcNow.AddDays(30),
                EndDate = DateTimeOffset.UtcNow.AddMonths(2),
                IsActive = true,
                Capacity = 50,
                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(2),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(20)
            };

            var courseId = await _courseRepository.AddAsync(course);

            var feeTemplate = new FeeTemplate
            {
                Name = "Undergraduate Tuition Template",
                CourseId = courseId,
                CalculationType = CalculationType.RatePerCredit,
                Amount = 50000.00m,
                RatePerCredit = 2500.00m,        // 
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = null
            };

            #endregion
            var templateId = await _feeTemplateRepository.AddAsync(feeTemplate);

            Assert.IsNotNull(templateId);
            Assert.AreNotEqual(0, templateId);

        }

        [TestMethod]
        public async Task GettAllAsync_IfNotNullThen_ReturnListOfFeeTemplate()
        {
            #region Arrange
            var course = new Course
            {
                Code = "CS101",
                Title = "Introduction to Programming 2",
                Credits = 3,
                Description = "Fundamentals of programming using C# and .NET Core.",
                Instructor = "Dr. Anil Sharma",
                StartDate = DateTimeOffset.UtcNow.AddDays(30),
                EndDate = DateTimeOffset.UtcNow.AddMonths(2),
                IsActive = true,
                Capacity = 50,
                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(2),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(20)
            };

            var courseId = await _courseRepository.AddAsync(course);

            var feeTemplate = new FeeTemplate
            {
                Name = " Tuition Template",
                CourseId = courseId,
                CalculationType = CalculationType.RatePerCredit,
                Amount = 50000.00m,
                RatePerCredit = 2500.00m,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = null
            };
            await _feeTemplateRepository.AddAsync(feeTemplate);
            #endregion

            //Act 
            var feetemplates = await _feeTemplateRepository.GetAllAsync();

            //Assert 
            Assert.IsNotNull(feetemplates);
            Assert.AreNotEqual(0, feetemplates.Count());
        }

        [TestMethod]
        public async Task GetByIdAsync_WithExistingId_ReturnsFeeTemplate()
        {
            #region Arrange
            var course = new Course
            {
                Code = "CS101",
                Title = "Introduction to Programming 3",
                Credits = 3,
                Description = "Fundamentals of programming using C# and .NET Core.",
                Instructor = "Dr. Anil Sharma",
                StartDate = DateTimeOffset.UtcNow.AddDays(30),
                EndDate = DateTimeOffset.UtcNow.AddMonths(2),
                IsActive = true,
                Capacity = 50,
                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(2),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(20)
            };

            var courseId = await _courseRepository.AddAsync(course);

            var feeTemplate = new FeeTemplate
            {
                Name = " Tuition Template",
                CourseId = courseId,
                CalculationType = CalculationType.RatePerCredit,
                Amount = 50000.00m,
                RatePerCredit = 2500.00m,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = null
            };

            var feeTemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);

            #endregion

            //act 
            var feeTempalateData = await _feeTemplateRepository.GetByIdAsync(feeTemplateId);


            Assert.IsNotNull(feeTempalateData);

        }

        [TestMethod]
        public async Task UpdateAsync_WithExistingId_ReturnsTrue_AndUpdatesData()
        {

            // Arrange
            var course = new Course
            {
                Code = "CS201",
                Title = "Data Structures",
                Credits = 4,
                Description = "DS course",
                Instructor = "Dr. Kumar",
                StartDate = DateTimeOffset.UtcNow.AddDays(10),
                EndDate = DateTimeOffset.UtcNow.AddMonths(3),
                IsActive = true,
                Capacity = 40,
                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(1),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(15)
            };

            var courseId = await _courseRepository.AddAsync(course);

            var feeTemplate = new FeeTemplate
            {
                Name = "Original Fee Template",
                CourseId = courseId,
                CalculationType = CalculationType.FlatAmount,
                Amount = 40000m,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow
            };

            var feeTemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);

            var updateData = new FeeTemplate
            {
                Name = "Updated Fee Template",
                Amount = 45000m,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            // Act
            var isUpdated = await _feeTemplateRepository.UpdateAsync(feeTemplateId, updateData);
            var updatedTemplate = await _feeTemplateRepository.GetByIdAsync(feeTemplateId);

            // Assert
            Assert.IsTrue(isUpdated);
            Assert.IsNotNull(updatedTemplate);
            Assert.AreEqual("Updated Fee Template", updatedTemplate.Name);
            Assert.AreEqual(45000m, updatedTemplate.Amount);
        }


        [TestMethod]
        public async Task DeleteAsync_WithActiveId_SetsIsActiveFalse()
        {
            #region Arrange
            var course = new Course
            {
                Code = "CS101",
                Title = "Introduction to Programming 4",
                Credits = 3,
                Description = "Fundamentals of programming using C# and .NET Core.",
                Instructor = "Dr. Anil Sharma",
                StartDate = DateTimeOffset.UtcNow.AddDays(30),
                EndDate = DateTimeOffset.UtcNow.AddMonths(2),
                IsActive = true,
                Capacity = 50,
                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(2),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(20)
            };

            var courseId = await _courseRepository.AddAsync(course);

            var feeTemplate = new FeeTemplate
            {
                Name = " Tuition Template 2",
                CourseId = courseId,
                CalculationType = CalculationType.RatePerCredit,
                Amount = 50000.00m,
                RatePerCredit = 2500.00m,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = null
            };

            #endregion

            var feeTemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);

            var isdeleted = await _feeTemplateRepository.DeleteAsync(feeTemplateId);

            Assert.AreEqual(false, isdeleted);

        }
        #endregion
    }
}
