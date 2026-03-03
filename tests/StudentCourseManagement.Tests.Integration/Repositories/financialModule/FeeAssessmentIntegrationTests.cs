using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper;
using StudentCourseManagement.Data.Repositories.Dapper.FinancialModule;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Common.Builders;
using StudentCourseManagement.Tests.Common.Builders.FinancialModule;
using System.Transactions;

namespace StudentCourseManagement.Tests.Integration.Repositories.financialModule
{
    [TestClass]
    [DoNotParallelize]
    public class FeeAssessmentIntegrationTests
    {
        #region Private ReadOnly Field
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;
        private readonly EnrollmentRepository _enrollmentRepository;
        private readonly FeeTemplateRepository _feeTemplate;
        private readonly FeeAssessmentRepository _feeAssessment;
        private readonly InvoiceRepository _invoiceRepository;
        private TransactionScope _scope;
        #endregion

        [TestInitialize]
        public void Init()
        {
            _scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _scope.Dispose();
        }

        public FeeAssessmentIntegrationTests()
        {
            var dbFixture = new DatabaseFixture();
            var mockLoggerStudent = new Mock<ILogger<StudentRepository>>();
            var mockLoggerCourse = new Mock<ILogger<CourseRepository>>();
            var mockLoggerEnrollment = new Mock<ILogger<EnrollmentRepository>>();
            var loggerMock = new Mock<ILogger<FeeAssessmentRepository>>();
            var mockLoggerFeeTemplate = new Mock<ILogger<FeeTemplateRepository>>();
            var mockLoggerInvoice = new Mock<ILogger<InvoiceRepository>>();

            _studentRepository = new StudentRepository(dbFixture.DbContext, mockLoggerStudent.Object);
            _courseRepository = new CourseRepository(dbFixture.DbContext, mockLoggerCourse.Object);
            _enrollmentRepository = new EnrollmentRepository(dbFixture.DbContext, mockLoggerEnrollment.Object);
            _feeTemplate = new FeeTemplateRepository(dbFixture.DbContext, mockLoggerFeeTemplate.Object);
            _feeAssessment = new FeeAssessmentRepository(dbFixture.DbContext, loggerMock.Object);
            _invoiceRepository = new InvoiceRepository(dbFixture.DbContext, mockLoggerInvoice.Object);
        }

        #region CURD Operations 

        [TestMethod]
        public async Task AddAsync_WithValidData_InsertData()
        {
            //Arrange
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
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);

            var updateFeeAssessment = new FeeAssessmentBuilder()
                .WithFeeAssessmentId(feeAssessmentId).WithEnrollmentId(enrollmentId).WithCourseId(courseId)
                .WithFeeTemplateId(feeTemplateId).WithFeeAssessmentStatus(AssessmentStatus.Assessed).Build();

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

        #region Phase -4 Required Method 
        [TestMethod]
        public async Task GetByInvoiceIdAsync_IfExistingInvoiceIdExists_ReturnFeeAssessmentData()
        {
            //Arrange 
            var studentId = await CreateStudentAsync();
            Assert.IsGreaterThan(0, studentId);
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feetemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feetemplateId);

            var invoice = new Invoice
            {
                FeeAssessmentId = feeAssessmentId,
                CourseId = courseId,
                IsActive = true,
                StudentId = studentId,
            };
            var invoiceId = await _invoiceRepository.AddAsync(invoice);


            //Act 
            var result = await _feeAssessment.GetByInvoiceIdAsync(invoiceId);
            //Assert 
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<FeeAssessment>(result);
        }

        #endregion

        #region Private Helper Methods 

        private async Task<int> CreateStudentAsync()
        {
            var student = new StudentBuilder().
                WithName("Ram NAth ").Build();
            return await _studentRepository.AddAsync(student);
        }

        private async Task<int> CreateCourseAsync()
        {
            var course = new CourseBuilder().Build();
            return await _courseRepository.AddAsync(course);
        }
        private async Task<int> CreateEnrollmentAsync(int studentId, int courseId)
        {
            var enrollment = new EnrollmentBuilder()
                .WithCourseId(courseId).WithStudentId(studentId).Build();
            return await _enrollmentRepository.AddAsync(enrollment);

        }
        private async Task<int> CreateFeeTemplateAsync(int courseId)
        {
            var feeTemplate = new FeeTemplateBuilder()
                .WithCourseId(courseId).WithCalculationType(CalculationType.RatePerCredit)
                .Build();
            return await _feeTemplate.AddAsync(feeTemplate);
        }

        private async Task<int> CreateFeeAssessmentAsync(int enrollmentid, int courseId, int feeTemplateId)
        {

            var feeAssessment = new FeeAssessmentBuilder()
                .WithEnrollmentId(enrollmentid).WithCourseId(courseId).WithFeeTemplateId(feeTemplateId).WithIsActive(true)
                .Build();

            return await _feeAssessment.AddAsync(feeAssessment);
        }
        #endregion
    }
}
