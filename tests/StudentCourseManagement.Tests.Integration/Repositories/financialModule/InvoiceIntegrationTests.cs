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
    public class InvoiceIntegrationTests
    {
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;
        private readonly EnrollmentRepository _enrollmentRepository;
        private readonly FeeAssessmentRepository _feeAssessmentRepository;
        private readonly InvoiceRepository _invoiceRepository;
        private readonly FeeTemplateRepository _feeTemplate;
        private TransactionScope _scope;

        [TestInitialize]
        public void Init()
        {
            _scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _scope.Dispose(); // rollback
        }



        public InvoiceIntegrationTests()
        {
            var dbFixture = new DatabaseFixture();

            var mockLoggerStudent = new Mock<ILogger<StudentRepository>>();
            var mockLoggerCourse = new Mock<ILogger<CourseRepository>>();
            var mockLoggerEnrollment = new Mock<ILogger<EnrollmentRepository>>();
            var mockLoggerFeeAssessment = new Mock<ILogger<FeeAssessmentRepository>>();
            var mockLoggerFeeTemplate = new Mock<ILogger<FeeTemplateRepository>>();
            var mockLogger = new Mock<ILogger<InvoiceRepository>>();

            _studentRepository = new StudentRepository(dbFixture.DbContext, mockLoggerStudent.Object);
            _courseRepository = new CourseRepository(dbFixture.DbContext, mockLoggerCourse.Object);
            _enrollmentRepository = new EnrollmentRepository(dbFixture.DbContext, mockLoggerEnrollment.Object);
            _feeTemplate = new FeeTemplateRepository(dbFixture.DbContext, mockLoggerFeeTemplate.Object);
            _feeAssessmentRepository = new FeeAssessmentRepository(dbFixture.DbContext, mockLoggerFeeAssessment.Object);
            _invoiceRepository = new InvoiceRepository(dbFixture.DbContext, mockLogger.Object);

        }

        #region CURD Operations 
        [TestMethod]
        public async Task AddAsync_WithValid_InsertsRowAndReturnsId()
        {
            //Arrange

            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);
            //Act
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);

            //Assert
            Assert.IsGreaterThan(0, invoiceId);

        }

        [TestMethod]
        public async Task GettAllAsync_IfNotNullThen_ReturnListOfInvoice()
        {
            //Arrange
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);
            await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);

            //Act 
            var invocies = await _invoiceRepository.GetAllAsync();

            //Assert
            Assert.IsTrue(invocies.Any());
        }

        [TestMethod]
        public async Task GetByIdAsync_WithExistingId_ReturnsInvoice()
        {
            //Arrange

            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);

            //Act
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);

            //Assert
            Assert.IsNotNull(invoice);
        }

        [TestMethod]
        public async Task UpdateAsync_WithExistingId_ReturnsTrue_AndUpdatesData()
        {
            //Arrange

            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);


            var updateInvoiceData = new InvoiceBuilder()
                .WithInvoiceId(invoiceId).WithCourseId(courseId)
                .WithFeeAssessmentId(feeAssessmentId).WithStudentId(studentId)
                .WithUpdatedAt(DateTimeOffset.UtcNow).Build();

            //Act 

            var result = await _invoiceRepository.UpdateAsync(invoiceId, updateInvoiceData);

            //Assert 
            Assert.IsTrue(result);

            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);

            Assert.AreEqual(updateInvoiceData.UpdatedAt, invoice?.UpdatedAt);
            Assert.AreEqual(updateInvoiceData.InvoiceStatus, invoice?.InvoiceStatus);
        }

        [TestMethod]
        public async Task DeleteAsync_WithActiveId_SetsIsActiveFalse()
        {
            //Arrange

            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);

            var setIsActiveFalse = await _invoiceRepository.DeleteAsync(invoiceId);
            //Act 
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);

            //Assert
            Assert.IsTrue(setIsActiveFalse);
            Assert.IsNull(invoice);
        }

        #endregion

        #region Phase 3 required methods 
        [TestMethod]
        public async Task GetByFeeAssessmentIdAsync_WithExistingFeeAssessmentId_ReturnsFeeAssessment()
        {
            //Arrange 
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feetemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feetemplateId);

            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);

            //act
            var result = await _invoiceRepository.GetByFeeAssessmentIdAsync(feeAssessmentId);
            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<Invoice>(result);

        }
        #endregion

        #region Helper Method

        private async Task<int> CreateStudentAsync()
        {
            var student = new StudentBuilder().Build();
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
                .WithEnrollmentId(enrollmentid).WithCourseId(courseId).WithFeeTemplateId(feeTemplateId)
                .Build();

            return await _feeAssessmentRepository.AddAsync(feeAssessment);
        }
        private async Task<int> CreateInvoiceAsync(int studentId, int courseId, int feeAssessmentId)
        {
            var invoice = new InvoiceBuilder()
                .WithStudentId(studentId).WithCourseId(courseId).WithFeeAssessmentId(feeAssessmentId)
                .Build();
            return await _invoiceRepository.AddAsync(invoice);
        }
        #endregion

    }
}
