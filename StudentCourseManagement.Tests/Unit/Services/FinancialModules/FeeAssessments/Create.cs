using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.FeeAssessments
{
    [TestClass]
    public class Create : FeeAssessmentServiceTestBase
    {
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
                Title = "C#",
                IsActive = true
            };
            return await _courseRepository.AddAsync(course);

        }
        private async Task<int> CreateFeeTemplate(int courseId)
        {
            var feeTemplate = new FeeTemplate
            {
                CourseId = courseId,
                Amount = 50000,
                IsActive = true
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
                EnrollmentStatus = EnrollmentStatus.Cancelled
            };
            return await _enrollmentRepository.AddAsync(enrollment);
        }

        #endregion
    }
}
