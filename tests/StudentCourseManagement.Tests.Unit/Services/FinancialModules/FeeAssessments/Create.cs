using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.FeeAssessments
{
    [TestClass]
    public class Create : FeeAssessmentServiceTestBase
    {
        #region Manual Assessment Creating test 

        [TestMethod]
        public async Task CreateAsync_WithValidEnrollment_ReturnsTrue()
        {

            //Arrange 

            var studentId = await CreateStudent();
            var courseId = await CreateCourse();
            var enrollmentId = await CreateEnrollment(courseId, studentId);
            var feeTemplateId = await CreateFeeTemplate(courseId);


            var feeAssessment = new FeeAssessmentBuilder()
                   .WithCourseId(courseId).WithFeeTemplateId(feeTemplateId)
                   .WithEnrollmentId(enrollmentId).WithAmount(1000m).Build();
            //Act 

            var (success, errorMessage, feeAssessmentId) = await _feeAssessmentService.CreateAsync(feeAssessment);

            //Assert 
            Assert.IsTrue(success);
            Assert.IsNull(errorMessage);

            var saved = await _feeAssessmentService.GetByIdAsync(1);
            Assert.IsNotNull(saved);
        }

        [TestMethod]
        public async Task CreateAsync_WhenCourseNotFound_ReturnsFalse()
        {
            //Arrange 
            var studentId = await CreateStudent();

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                IsActive = true
            };

            var enrollmentId = await _enrollmentRepository.AddAsync(enrollment);

            var feeTemplate = new FeeTemplate
            {
                IsActive = true,
                Amount = 1000
            };

            var feeTemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);

            var feeAssessment = new FeeAssessmentBuilder()
                    .WithFeeTemplateId(feeTemplateId)
                    .WithEnrollmentId(enrollmentId).WithAmount(1000m).Build();

            //Act 

            var (success, errorMessage, feeAssessmentId) = await _feeAssessmentService.CreateAsync(feeAssessment);

            //Assert 
            Assert.IsFalse(success);

            var saved = await _feeAssessmentService.GetByIdAsync(feeAssessment.FeeAssessmentId + 1);
            Assert.IsNull(saved);
        }

        [TestMethod]
        public async Task CreateAsync_WhenEnrollmentNotFound_ReturnsFalse()
        {
            //Arrange 
            var courseId = await CreateCourse();
            var feetemplateId = await CreateFeeTemplate(courseId);

            var feeAssessment = new FeeAssessmentBuilder()
                     .WithCourseId(courseId).WithFeeTemplateId(feetemplateId)
                     .WithAmount(1000m).Build();

            //Act 
            var (success, errorMessage, feeAssessmentId) = await _feeAssessmentService.CreateAsync(feeAssessment);

            //Assert 
            Assert.IsFalse(success);
        }

        [TestMethod]
        public async Task CreateAsync_WhenFeeTemplateNotFound_ReturnsFalse()
        {
            //Arrange 
            var studentId = await CreateStudent();
            var courseId = await CreateCourse();
            var enrollmentId = await CreateEnrollment(courseId, studentId);
            var feeAssessment = new FeeAssessment
            {
                CourseId = courseId,
                EnrollmentId = enrollmentId,
                Amount = 1000,
                IsActive = true
            };

            //Act 
            var (success, errorMessage, feeAssessmentId) = await _feeAssessmentService.CreateAsync(feeAssessment);

            //Assert 
            Assert.IsFalse(success);

        }
        #endregion

        #region Phase 3 : Automated FeeAssessment and Invoice Generation tests

        [TestMethod]
        public async Task AssessFee_WithValidData_ReturnsTrue()
        {
            //Arrange 
            var studentId = await CreateStudent();
            var courseId = await CreateCourse();
            await CreateFeeTemplate(courseId);

            var enrollment = new EnrollmentBuilder()
                .WithStudentId(studentId).WithCourseId(courseId)
                .WithEnrollmentStatus(EnrollmentStatus.Comfirmed).WithEnrollmentDate(DateTimeOffset.UtcNow)
                .Build();

            var enrollmentId = await _enrollmentRepository.AddAsync(enrollment);

            //Act 
            var result = await _feeAssessmentService.AssessFee(enrollmentId);
            //Assert 
            Assert.IsTrue(result.success);
            Assert.IsNull(result.ErrorMessage);

        }

        [TestMethod]
        public async Task AssessFee_EnrollmentNotFound_ReturnsFalse()
        {
            var courseId = await CreateCourse();
            await CreateFeeTemplate(courseId);
            int enrollment = 1;

            var result = await _feeAssessmentService.AssessFee(enrollment);

            Assert.IsFalse(result.success);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [TestMethod]
        public async Task AssessFee_EnrollmentNotConfirmed_ReturnsFalse()
        {
            //Arrange 
            var studentId = await CreateStudent();
            var courseId = await CreateCourse();
            await CreateFeeTemplate(courseId);
            var enrollment = new EnrollmentBuilder()
                .WithStudentId(studentId).WithCourseId(courseId)
                .WithEnrollmentStatus(EnrollmentStatus.Cancelled).WithEnrollmentDate(DateTimeOffset.UtcNow)
                .Build();
            var enrollmentId = await _enrollmentRepository.AddAsync(enrollment);

            //Act
            var result = await _feeAssessmentService.AssessFee(enrollmentId);
            //Assert
            Assert.IsFalse(result.success);
            Assert.IsNotNull(result.ErrorMessage);

        }
        [TestMethod]
        public async Task AssessFee_AlreadyAssessed_ReturnsFalse()
        {
            //Arrange 
            var studentId = await CreateStudent();
            var courseId = await CreateCourse();
            await CreateFeeTemplate(courseId);
            var enrollmentId = await CreateEnrollment(studentId, courseId);
            await _feeAssessmentService.AssessFee(enrollmentId);
            //Act
            var result = await _feeAssessmentService.AssessFee(enrollmentId);
            ////Assert
            Assert.IsFalse(result.success);
            Assert.IsNotNull(result.ErrorMessage);

        }

        [TestMethod]
        public async Task AssessFee_FeeTemplateNotFound_ReturnsFalse()
        {
            //Arrange 
            var studentId = await CreateStudent();
            var courseId = await CreateCourse();

            var enrollmentId = await CreateEnrollment(studentId, courseId);
            //Act
            var result = await _feeAssessmentService.AssessFee(enrollmentId);
            //Assert
            Assert.IsFalse(result.success);
            Assert.IsNotNull(result.ErrorMessage);

        }
        [TestMethod]
        public async Task AssessFee_FeeTemplateInactive_ReturnsFalse()
        {
            //Arrange 
            var studentId = await CreateStudent();
            var courseId = await CreateCourse();

            var feeTemplate = new FeeTemplateBuilder()
                .WithCourseId(courseId).WithCalculationType(CalculationType.FlatAmount).WithIsActive(false).Build();

            var feeTemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);

            var enrollmentId = await CreateEnrollment(studentId, courseId);
            //Act
            var result = await _feeAssessmentService.AssessFee(enrollmentId);
            //Assert 
            Assert.IsFalse(result.success);
            Assert.IsNotNull(result.ErrorMessage);

        }

        [TestMethod]
        public async Task AssessFee_FlatAmount_CalculatesCorrectly()
        {
            //Arrange 
            var studentId = await CreateStudent();
            var course = new CourseBuilder()
                .WithCredits(0).WithTitle("Introduction to Computer Science").Build();

            var courseId = await _courseRepository.AddAsync(course);

            var feeTemplate = new FeeTemplateBuilder()
                .WithCourseId(courseId).WithCalculationType(CalculationType.FlatAmount)
                .WithRatePerCredit(0).Build();

            var feeTemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);

            var enrollmentId = await CreateEnrollment(studentId, courseId);

            //Act
            var result = await _feeAssessmentService.AssessFee(enrollmentId);

            //Assert 
            var feeeAssessment = await _feeTemplateRepository.GetByIdAsync(enrollmentId);
            Assert.AreEqual(feeTemplate.Amount, feeeAssessment?.Amount);
        }
        [TestMethod]
        public async Task AssessFee_RatePerCredit_CalculatesCorrectly()
        {
            //Arrange 
            var studentId = await CreateStudent();
            var course = new CourseBuilder()
                .WithCredits(0).WithTitle("Introduction to Computer Science").Build();

            var courseId = await _courseRepository.AddAsync(course);

            var feeTemplate = new FeeTemplateBuilder()
                .WithCourseId(courseId).WithCalculationType(CalculationType.RatePerCredit)
                .WithRatePerCredit(100).Build();
            var feeTemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);


            var enrollmentId = await CreateEnrollment(courseId, studentId);

            //Act
            var result = await _feeAssessmentService.AssessFee(enrollmentId);

            //Assert 
            var feeeAssessment = await _feeAssessmentService.GetByIdAsync(enrollmentId);
            Assert.AreEqual(feeTemplate.RatePerCredit * course.Credits, feeeAssessment?.Amount);

        }

        [TestMethod]
        public async Task AssessFee_CreatesInvoice_WithCorrectValues()
        {
            //Arrange 
            var studentId = await CreateStudent();
            var courseId = await CreateCourse();
            await CreateFeeTemplate(courseId);
            var enrollmentId = await CreateEnrollment(courseId, studentId);

            //Act 
            var result = await _feeAssessmentService.AssessFee(enrollmentId);
            //Assert 
            Assert.IsTrue(result.success);
            // requires new methosd to check : GetInvoiceByEnrollmentId - may be join may used 


        }
        [TestMethod]
        public async Task AssessFee_UpdatesEnrollment_UpdatesFeeAssessedDate()
        {
            //Arrange 
            var studentId = await CreateStudent();
            var courseId = await CreateCourse();
            await CreateFeeTemplate(courseId);
            var enrollmentId = await CreateEnrollment(studentId, courseId);

            //Act
            var result = await _feeAssessmentService.AssessFee(enrollmentId);

            //Arrange
            Assert.IsTrue(result.success, "AssessFee did not succeed");
            var enrollmentData = await _enrollmentRepository.GetByIdAsync(enrollmentId);
            Assert.IsNotNull(enrollmentData?.FeeAssessmentDate);
            var delta = DateTimeOffset.UtcNow - enrollmentData.FeeAssessmentDate.Value;
            Assert.IsTrue(Math.Abs(delta.TotalSeconds) < 2, $"FeeAssessmentDate differs by {delta.TotalSeconds} seconds");

        }

        #endregion

        #region Private Helper Methods
        private async Task<int> CreateStudent()
        {
            var student = new StudentBuilder()
                      .Build();
            return await _studentRepository.AddAsync(student);

        }
        private async Task<int> CreateCourse()
        {
            var course = new CourseBuilder()
                .Build();
            return await _courseRepository.AddAsync(course);

        }
        private async Task<int> CreateFeeTemplate(int courseId)
        {
            var feeTemplate = new FeeTemplateBuilder()
                .WithCourseId(courseId).WithAmount(4000).
                WithRatePerCredit(100).Build();

            return await _feeTemplateRepository.AddAsync(feeTemplate);
        }
        private async Task<int> CreateEnrollment(int courseId, int studentId)
        {
            var enrollement = new EnrollmentBuilder()
                .WithStudentId(studentId).WithCourseId(courseId)
                .WithEnrollmentStatus(EnrollmentStatus.Comfirmed).WithEnrollmentDate(DateTimeOffset.UtcNow).Build();

            return await _enrollmentRepository.AddAsync(enrollement);
        }

        #endregion
    }
}
