using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper;
using StudentCourseManagement.Data.Repositories.Dapper.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Common.Builders;
using StudentCourseManagement.Tests.Common.Builders.FinancialModule;
using System.Transactions;

namespace StudentCourseManagement.Tests.Integration.Repositories.financialModule
{
    [TestClass]
    [DoNotParallelize]
    public class InvoiceLineItemIntegrationTests
    {
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;
        private readonly EnrollmentRepository _enrollmentRepository;
        private readonly FeeAssessmentRepository _feeAssessmentRepository;
        private readonly InvoiceRepository _invoiceRepository;
        private readonly FeeTemplateRepository _feeTemplate;
        private readonly InvoiceLineItemRepository _invoiceLineItemRepository;
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


        public InvoiceLineItemIntegrationTests()
        {
            var dbFixture = new DatabaseFixture();

            var mockLoggerStudent = new Mock<ILogger<StudentRepository>>();
            var mockLoggerCourse = new Mock<ILogger<CourseRepository>>();
            var mockLoggerEnrollment = new Mock<ILogger<EnrollmentRepository>>();
            var mockLoggerFeeAssessment = new Mock<ILogger<FeeAssessmentRepository>>();
            var mockLoggerFeeTemplate = new Mock<ILogger<FeeTemplateRepository>>();
            var mockLogger = new Mock<ILogger<InvoiceRepository>>();
            var mockLoggerInvoiceLineItem = new Mock<ILogger<InvoiceLineItemRepository>>();

            _studentRepository = new StudentRepository(dbFixture.DbContext, mockLoggerStudent.Object);
            _courseRepository = new CourseRepository(dbFixture.DbContext, mockLoggerCourse.Object);
            _enrollmentRepository = new EnrollmentRepository(dbFixture.DbContext, mockLoggerEnrollment.Object);
            _feeTemplate = new FeeTemplateRepository(dbFixture.DbContext, mockLoggerFeeTemplate.Object);
            _feeAssessmentRepository = new FeeAssessmentRepository(dbFixture.DbContext, mockLoggerFeeAssessment.Object);
            _invoiceRepository = new InvoiceRepository(dbFixture.DbContext, mockLogger.Object);
            _invoiceLineItemRepository = new InvoiceLineItemRepository(dbFixture.DbContext, mockLoggerInvoiceLineItem.Object);

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
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);

            //Act 
            var invoiceLineItemId = await CreateInvoiceLineItemAsync(invoiceId, courseId, feeTemplateId);

            //Arrange 
            Assert.IsGreaterThan(0, invoiceLineItemId);

        }

        [TestMethod]
        public async Task GettAllAsync_IfNotNullThen_ReturnListOfInvoiceLineItem()
        {
            //Arrange 
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);
            await CreateInvoiceLineItemAsync(invoiceId, courseId, feeTemplateId);

            //Act 
            var invoiceLineItems = await _invoiceLineItemRepository.GetAllAsync();

            //Arrange 
            Assert.IsTrue(invoiceLineItems.Any());

        }

        [TestMethod]
        public async Task GetByIdAsync_WithExistingId_ReturnsInvoiceLineItem()
        {
            //Arrange 

            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);
            var invoiceLineItemId = await CreateInvoiceLineItemAsync(invoiceId, courseId, feeTemplateId);

            //Act 
            var invoiceLineItem = await _invoiceLineItemRepository.GetByIdAsync(invoiceLineItemId);

            //Arrange 
            Assert.IsNotNull(invoiceLineItem);

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
            var invoiceLineItemId = await CreateInvoiceLineItemAsync(invoiceId, courseId, feeTemplateId);

            var updated = new InvoiceLineItemBuilder()
                .WithInvoiceLineItemId(invoiceLineItemId).WithInvoiceId(invoiceId)
                .WithCourseId(courseId).WithFeeTemplateId(feeTemplateId)
                .WithDescription("Updated DEscription").Build();

            //Act 
            var result = await _invoiceLineItemRepository.UpdateAsync(invoiceLineItemId, updated);


            // Assert
            Assert.IsTrue(result);

            var fromDb = await _invoiceLineItemRepository.GetByIdAsync(invoiceLineItemId);
            Assert.AreEqual(updated.Description, fromDb?.Description);
            Assert.AreEqual(updated.Quantity, fromDb?.Quantity);


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

            //Act 
            var invoiceLineItemId = await CreateInvoiceLineItemAsync(invoiceId, courseId, feeTemplateId);

            var result = await _invoiceLineItemRepository.DeleteAsync(invoiceLineItemId);

            var invoiceLineItem = await _invoiceLineItemRepository.GetByIdAsync(invoiceLineItemId);

            //Assert 
            Assert.IsTrue(result);
            Assert.IsNull(invoiceLineItem);



        }

        #endregion

        #region Private Helper Methods

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

        private async Task<int> CreateInvoiceLineItemAsync(int invoiceId, int courseId, int feeTemplateId)
        {
            var lineItem = new InvoiceLineItemBuilder()
                .WithCourseId(courseId).WithFeeTemplateId(feeTemplateId)
                .WithInvoiceId(invoiceId).Build();

            return await _invoiceLineItemRepository.AddAsync(lineItem);

        }
        #endregion
    }
}
