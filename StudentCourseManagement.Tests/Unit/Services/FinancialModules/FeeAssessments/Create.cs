using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

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


            var feeAssessment = new FeeAssessment
            {
                FeeAssessmentId = 0,
                CourseId = courseId,
                FeeTemplateId = feeTemplateId,
                EnrollmentId = enrollmentId,
                IsActive = true,
                Amount = 1000
            };

            //Act 

            var result = await _feeAssessmentService.CreateAsync(feeAssessment);

            //Assert 
            Assert.IsTrue(result);

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

            var feeAssessment = new FeeAssessment
            {
                FeeAssessmentId = 0,
                FeeTemplateId = feeTemplateId,
                EnrollmentId = enrollmentId,
                IsActive = true,
                Amount = 1000
            };

            //Act 

            var result = await _feeAssessmentService.CreateAsync(feeAssessment);

            //Assert 
            Assert.IsFalse(result);

            var saved = await _feeAssessmentService.GetByIdAsync(feeAssessment.FeeAssessmentId + 1);
            Assert.IsNull(saved);
        }

        [TestMethod]
        public async Task CreateAsync_WhenEnrollmentNotFound_ReturnsFalse()
        {
            //Arrange 
            var courseId = await CreateCourse();
            var feetemplateId = await CreateFeeTemplate(courseId);
            var feeAssessment = new FeeAssessment
            {
                CourseId = courseId,
                FeeTemplateId = feetemplateId,
                Amount = 1000,
                IsActive = true
            };

            //Act 
            var isCreated = await _feeAssessmentService.CreateAsync(feeAssessment);

            //Assert 
            Assert.IsFalse(isCreated);
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
            var isCreated = await _feeAssessmentService.CreateAsync(feeAssessment);

            //Assert 
            Assert.IsFalse(isCreated);

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
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                IsActive = true,
                EnrollmentStatus = EnrollmentStatus.Comfirmed,
                EnrollmentDate = DateTimeOffset.UtcNow
            };
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
            var enrollmentId = await CreateEnrollment(studentId, courseId);

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
            var feeTemplate = new FeeTemplate
            {
                CourseId = courseId,
                CalculationType = CalculationType.FlatAmount,
                Amount = 500,
                CreatedAt = DateTimeOffset.UtcNow,
                Name = "Lab fee",
                IsActive = false
            };
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
            var course = new Course
            {
                CourseId = 501,
                Code = "CS101",
                Title = "Introduction to Computer Science",
                IsActive = true,
                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(4),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(14),
                StartDate = DateTimeOffset.UtcNow.AddDays(30),
                EndDate = DateTimeOffset.UtcNow.AddMonths(2),
                Capacity = 20
            };
            var courseId = await _courseRepository.AddAsync(course);

            var feeTemplate = new FeeTemplate
            {
                CourseId = courseId,
                CalculationType = CalculationType.FlatAmount,
                CreatedAt = DateTimeOffset.UtcNow,
                Amount = 5000,
                IsActive = true,
                Name = "Lab fee",
                RatePerCredit = 0
            };
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
            var course = new Course
            {
                CourseId = 501,
                Code = "CS101",
                Title = "Introduction to Computer Science",
                Credits = 5,
                IsActive = true,
                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(4),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(14),
                StartDate = DateTimeOffset.UtcNow.AddDays(30),
                EndDate = DateTimeOffset.UtcNow.AddMonths(2),
                Capacity = 20
            };
            var courseId = await _courseRepository.AddAsync(course);

            var feeTemplate = new FeeTemplate
            {
                CourseId = courseId,
                CalculationType = CalculationType.RatePerCredit,
                CreatedAt = DateTimeOffset.UtcNow,
                Amount = 0,
                IsActive = true,
                Name = "Lab fee",
                RatePerCredit = 1000
            };
            var feeTemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                IsActive = true,
                EnrollmentStatus = EnrollmentStatus.Comfirmed,
                EnrollmentDate = DateTimeOffset.UtcNow
            };
            var enrollmentId = await _enrollmentRepository.AddAsync(enrollment);

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
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                IsActive = true,
                EnrollmentStatus = EnrollmentStatus.Comfirmed,
                EnrollmentDate = DateTimeOffset.UtcNow
            };
            var enrollmentId = await _enrollmentRepository.AddAsync(enrollment);

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
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentStatus = EnrollmentStatus.Comfirmed,
                IsActive = true
            };
            var enrollmentId = await _enrollmentRepository.AddAsync(enrollment);

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
            var student = new Student
            {
                Name = "Pawan",
                Address = "Haldibari",
                IsActive = true
            };
            return await _studentRepository.AddAsync(student);

        }
        private async Task<int> CreateCourse()
        {
            var course = new Course
            {
                CourseId = 501,
                Code = "CS101",
                Title = "Introduction to Computer Science",
                Credits = 5,
                IsActive = true,
                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(4),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(14),
                StartDate = DateTimeOffset.UtcNow.AddDays(30),
                EndDate = DateTimeOffset.UtcNow.AddMonths(2),
                Capacity = 20
            };
            return await _courseRepository.AddAsync(course);

        }
        private async Task<int> CreateFeeTemplate(int courseId)
        {
            var feeTemplate = new FeeTemplate
            {
                CourseId = courseId,
                CalculationType = CalculationType.RatePerCredit,
                CreatedAt = DateTimeOffset.UtcNow,
                Amount = 0,
                IsActive = true,
                Name = "Lab fee",
                RatePerCredit = 1000
            };

            return await _feeTemplateRepository.AddAsync(feeTemplate);
        }
        private async Task<int> CreateEnrollment(int courseId, int studentId)
        {
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                IsActive = true,
                EnrollmentStatus = EnrollmentStatus.Comfirmed
            };
            return await _enrollmentRepository.AddAsync(enrollment);
        }

        #endregion
    }
}
