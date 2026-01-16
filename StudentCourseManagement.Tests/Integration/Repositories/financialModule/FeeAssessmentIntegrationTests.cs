using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper;
using StudentCourseManagement.Data.Repositories.Dapper.FinancialModule;
using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using System.Transactions;

namespace StudentCourseManagement.Tests.Integration.Repositories.financialModule
{
    [TestClass]
    public class FeeAssessmentIntegrationTests
    {
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;
        private readonly EnrollmentRepository _enrollmentRepository;
        private readonly FeeTemplateRepository _feeTemplate;
        private readonly FeeAssessmentRepository _feeAssessment;
        public FeeAssessmentIntegrationTests()
        {
            var dbFixture = new DatabaseFixture();
            var mockLoggerStudent = new Mock<ILogger<StudentRepository>>();
            var mockLoggerCourse = new Mock<ILogger<CourseRepository>>();
            var mockLoggerEnrollment = new Mock<ILogger<EnrollmentRepository>>();
            var loggerMock = new Mock<ILogger<FeeAssessmentRepository>>();
            var mockLoggerFeeTemplate = new Mock<ILogger<FeeTemplateRepository>>();

            _studentRepository = new StudentRepository(dbFixture.DbContext, mockLoggerStudent.Object);
            _courseRepository = new CourseRepository(dbFixture.DbContext, mockLoggerCourse.Object);
            _enrollmentRepository = new EnrollmentRepository(dbFixture.DbContext, mockLoggerEnrollment.Object);
            _feeTemplate = new FeeTemplateRepository(dbFixture.DbContext, mockLoggerFeeTemplate.Object);
            _feeAssessment = new FeeAssessmentRepository(dbFixture.DbContext, loggerMock.Object);
        }

        #region CURD Operations 

        [TestMethod]
        public async Task AddAsync_WithValidData_InsertData()
        {
            //Arrange
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);

            //Act 
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);

            //Assert 
            Assert.IsNotNull(feeAssessmentId);
            Assert.IsTrue(feeAssessmentId > 0);
        }

        [TestMethod]
        public async Task GettAllAsync_IfNotNullThen_ReturnListOfFeeAssessment()
        {
            //Arrange
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);

            //Act
            var feeAssessments = await _feeAssessment.GetAllAsync();

            Assert.IsNotNull(feeAssessments);
            Assert.AreNotEqual(0, feeAssessments.Count());
            Assert.IsTrue(feeAssessments.Any());
        }

        [TestMethod]
        public async Task GetByIdAsync_IfNotNull_ReturnFeeAssessment()
        {
            //Arrange
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);

            //Act 
            var feeAssesment = await _feeAssessment.GetByIdAsync(feeAssessmentId);

            //Asset 
            Assert.IsNotNull(feeAssesment);
        }
        [TestMethod]
        public async Task UpdateAsync_WithValidInsert_ReturnTrue()
        {
            //Arrange
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);


            var updateFeeAssessment = new FeeAssessment
            {
                FeeAssessmentId = feeAssessmentId,
                EnrollmentId = enrollmentId,
                CourseId = courseId,
                FeeTemplateId = feeTemplateId,
                Amount = 15000.00m,
                DueDate = DateTimeOffset.UtcNow.AddMonths(1),
                FeeAssessmentStatus = AssessmentStatus.Assessed,
                IsActive = true,
                PaidDate = null,
                LateFeeAmount = null,
                LateFeeAppliedDate = null
            };
            //Act 

            var isUpdated = await _feeAssessment.UpdateAsync(feeAssessmentId, updateFeeAssessment);

            //Assert 
            Assert.IsTrue(isUpdated);
            var feeAssessment = await _feeAssessment.GetByIdAsync(feeAssessmentId);
            Assert.AreEqual(updateFeeAssessment.FeeAssessmentStatus, feeAssessment?.FeeAssessmentStatus);

        }

        [TestMethod]
        public async Task DeleteAsync_WithExistingActiveId_ReturnsTrue()
        {
            //Arrange
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);

            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);

            //Act

            var isDeleted = await _feeAssessment.DeleteAsync(feeAssessmentId);

            //Assert 
            Assert.IsTrue(isDeleted);
            var feeAssessment = await _feeAssessment.GetByIdAsync(feeAssessmentId);
            Assert.IsNull(feeAssessment);

        }

        #endregion

        #region Phase -3 required method 
        [TestMethod]
        public async Task ExistsByEnrollmentIdAsync_WithExistingEnrollmentId_ReturnsTrue()
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            //Arrange 
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);

            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);

            //Act 
            var result = await _feeAssessment.ExistsByEnrollmentIdAsync(enrollmentId);

            //Assert 
            Assert.IsTrue(result);

        }
        [TestMethod]
        public async Task GetByEnrolmentIdAsync_WithExistingEnrollmentId_ReturnFeeAssessment()
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            //Arrange 
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);

            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);

            //Act 
            var result = await _feeAssessment.GetByEnrolmentIdAsync(enrollmentId);

            //Assert 
            Assert.IsNotNull(result);
            Assert.AreEqual(result.CourseId, courseId);
        }


        #endregion

        #region Private Helper Methods 

        private async Task<int> CreateStudentAsync()
        {
            var student = new Student
            {
                Name = "Sita Sharma",
                Email = "sita.sharma@example.com",
                DOB = new DateTimeOffset(2004, 05, 12, 0, 0, 0, TimeSpan.FromHours(5.75)),
                Number = 9812345678,
                IsActive = true,
                Gender = "Female",
                Address = "Biratnagar, Nepal"
            };
            return await _studentRepository.AddAsync(student);
        }

        private async Task<int> CreateCourseAsync()
        {
            var course = new Course
            {
                Code = "CS1001",
                Title = "Introduction to Programming",
                Credits = 3,
                Description = "Fundamentals of programming using C# and .NET Core.",
                Instructor = "Dr. Anil Sharma",
                StartDate = DateTimeOffset.UtcNow.AddDays(40),
                EndDate = DateTimeOffset.UtcNow.AddMonths(2),
                IsActive = true,
                Capacity = 50,
                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(10),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(25),
            };

            return await _courseRepository.AddAsync(course);
        }
        private async Task<int> CreateEnrollmentAsync(int studentId, int courseId)
        {
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                IsActive = true
            };

            return await _enrollmentRepository.AddAsync(enrollment);

        }
        private async Task<int> CreateFeeTemplateAsync(int courseId)
        {
            var feeTemplate = new FeeTemplate
            {
                Name = "Undergraduate Tuition Template",
                CourseId = courseId,
                CalculationType = CalculationType.RatePerCredit,
                RatePerCredit = 2500.00m,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = null,
                Amount = 0
            };
            return await _feeTemplate.AddAsync(feeTemplate);
        }

        private async Task<int> CreateFeeAssessmentAsync(int enrollmentid, int courseId, int FeeTemplateId)
        {
            var feeAssessment = new FeeAssessment
            {
                EnrollmentId = enrollmentid,
                CourseId = courseId,
                FeeTemplateId = FeeTemplateId,
                Amount = 15000.00m,
                DueDate = DateTime.UtcNow.AddDays(30),
                FeeAssessmentStatus = AssessmentStatus.Pending,
                IsActive = true,
                PaidDate = null,
                LateFeeAmount = null,
                LateFeeAppliedDate = null
            };
            return await _feeAssessment.AddAsync(feeAssessment);
        }
        #endregion
    }
}
