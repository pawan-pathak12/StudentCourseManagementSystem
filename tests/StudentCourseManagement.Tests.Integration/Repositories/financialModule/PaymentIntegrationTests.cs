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
    public class PaymentIntegrationTests
    {
        #region Private ReadOnly Field

        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;
        private readonly EnrollmentRepository _enrollmentRepository;
        private readonly FeeAssessmentRepository _feeAssessmentRepository;
        private readonly InvoiceRepository _invoiceRepository;
        private readonly FeeTemplateRepository _feeTemplate;
        private readonly PaymentRepository _paymentRepository;
        private readonly PaymentMethodRepository _paymentMethodRepository;
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
            _scope.Dispose(); // rollback
        }

        #region Default Constructor
        public PaymentIntegrationTests()
        {
            var dbFixture = new DatabaseFixture();

            var mockLoggerStudent = new Mock<ILogger<StudentRepository>>();
            var mockLoggerCourse = new Mock<ILogger<CourseRepository>>();
            var mockLoggerEnrollment = new Mock<ILogger<EnrollmentRepository>>();
            var mockLoggerFeeAssessment = new Mock<ILogger<FeeAssessmentRepository>>();
            var mockLoggerFeeTemplate = new Mock<ILogger<FeeTemplateRepository>>();
            var mockLogger = new Mock<ILogger<InvoiceRepository>>();
            var mockLoggerPayment = new Mock<ILogger<PaymentRepository>>();
            var mockLoggerPaymentMethod = new Mock<ILogger<PaymentMethodRepository>>();

            _studentRepository = new StudentRepository(dbFixture.DbContext, mockLoggerStudent.Object);
            _courseRepository = new CourseRepository(dbFixture.DbContext, mockLoggerCourse.Object);
            _enrollmentRepository = new EnrollmentRepository(dbFixture.DbContext, mockLoggerEnrollment.Object);
            _feeTemplate = new FeeTemplateRepository(dbFixture.DbContext, mockLoggerFeeTemplate.Object);
            _feeAssessmentRepository = new FeeAssessmentRepository(dbFixture.DbContext, mockLoggerFeeAssessment.Object);
            _invoiceRepository = new InvoiceRepository(dbFixture.DbContext, mockLogger.Object);
            _paymentRepository = new PaymentRepository(dbFixture.DbContext, mockLoggerPayment.Object);
            _paymentMethodRepository = new PaymentMethodRepository(dbFixture.DbContext, mockLoggerPaymentMethod.Object);

        }
        #endregion


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
            var paymentMethodId = await CreatePaymentMethodAsync();

            //Act 
            var paymentId = await CreatePaymentAsync(studentId, invoiceId, paymentMethodId);

            //Assert    
            Assert.IsGreaterThan(0, paymentId);
        }

        [TestMethod]
        public async Task GettAllAsync_IfNotNullThen_ReturnListOfPayment()
        {
            //Arrange
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);
            var paymentMethodId = await CreatePaymentMethodAsync();

            await CreatePaymentAsync(studentId, invoiceId, paymentMethodId);

            //Act 
            var payments = await _paymentRepository.GetAllAsync();

            //Assert
            Assert.IsTrue(payments.Any());
            Assert.IsNotNull(payments);

        }

        [TestMethod]
        public async Task GetByIdAsync_WithExistingId_ReturnsPayments()
        {
            //Arrange
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);
            var paymentMethodId = await CreatePaymentMethodAsync();
            var paymentId = await CreatePaymentAsync(studentId, invoiceId, paymentMethodId);

            //Act 
            var payment = await _paymentRepository.GetByIdAsync(paymentId);

            //Assert
            Assert.IsNotNull(payment);
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
            var paymentMethodId = await CreatePaymentMethodAsync();
            var paymentId = await CreatePaymentAsync(studentId, invoiceId, paymentMethodId);

            var updatedPayment = new PaymentBuilder()
                .WithStudentId(studentId).WithInvoiceId(invoiceId)
                .WithPaymentMethodId(paymentMethodId).WithAmount(1000).WithPaymentId(paymentId)
               .WithNotes("Testing ").WithProcessedBy("Ram ").Build();

            //Act 
            var result = await _paymentRepository.UpdateAsync(paymentId, updatedPayment);

            //Assert 
            Assert.IsTrue(result);

            var payment = await _paymentRepository.GetByIdAsync(paymentId);

            Assert.AreEqual(updatedPayment.Notes, payment?.Notes);
            Assert.AreEqual(updatedPayment.ProcessedBy, payment?.ProcessedBy);
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
            var paymentMethodId = await CreatePaymentMethodAsync();
            var paymentId = await CreatePaymentAsync(studentId, invoiceId, paymentMethodId);

            //Act
            var result = await _paymentRepository.DeleteAsync(paymentId);
            var payment = await _paymentRepository.GetByIdAsync(paymentId);

            //Assert 
            Assert.IsTrue(result);
            Assert.IsNull(payment);


        }
        #endregion

        #region Phase 4 required methods 
        [TestMethod]
        public async Task GetByInvoiceIdAsync_WithExistingInvoiceId_ReturnPaymentData()
        {
            //Arrange 
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);
            var paymentMethodId = await CreatePaymentMethodAsync();
            var paymentId = await CreatePaymentAsync(studentId, invoiceId, paymentMethodId);

            //Act 
            var result = await _paymentRepository.GetByInvoiceIdAsync(invoiceId);
            //Assert 
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<Payment>(result);
        }
        #endregion

        #region Phase 5 :Refund Procesing : Required Methods 
        [TestMethod]
        public async Task GetInvoiceByPaymentIdAsync_ShouldReturnInvoice_WhenPaymentExists()
        {
            // Arrange  
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);
            var paymentMethodId = await CreatePaymentMethodAsync();
            var paymentId = await CreatePaymentAsync(studentId, invoiceId, paymentMethodId);
            // Act  
            var invoice = await _paymentRepository.GetInvoiceByPaymentIdAsync(paymentId);
            // Assert
            Assert.IsNotNull(invoice);
            Assert.IsInstanceOfType<Invoice>(invoice);
        }

        [TestMethod]
        public async Task IsRefundedAsync_ShouldReturnTrue_WhenPaymentHasRefund()
        {
            // Arrange  
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);
            var paymentMethodId = await CreatePaymentMethodAsync();
            var paymentId = await CreatePaymentAsync(studentId, invoiceId, paymentMethodId);
            var refund = new Payment
            {
                InvoiceId = invoiceId,
                RefundedPaymentId = paymentId,
                IsActive = true,
                Amount = -1000,
                StudentId = studentId,
                PaymentMethodId = paymentMethodId,
                RefundDate = DateTimeOffset.UtcNow
            };
            var refundId = await _paymentRepository.AddAsync(refund);
            // Act  
            var result = await _paymentRepository.IsRefundedAsync(paymentId);
            // Assert
        }

        [TestMethod]
        public async Task GetEnrollmentIdFromPaymentIdAsync_ShouldReturnEnrollmentId_WhenPaymentExists()
        {
            // Arrange  
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);
            var paymentMethodId = await CreatePaymentMethodAsync();
            var paymentId = await CreatePaymentAsync(studentId, invoiceId, paymentMethodId);

            // Act  
            var retriveEnrollmentId = await _paymentRepository.GetEnrollmentIdFromPaymentIdAsync(paymentId);
            // Assert
            Assert.IsGreaterThan(0, retriveEnrollmentId);
            Assert.AreEqual(enrollmentId, retriveEnrollmentId);

        }
        [TestMethod]
        public async Task GetRefundPaymentDataByPaymentIdAsync_ShouldReturnRefundPayment_WhenValidPaymentIdProvided()
        {
            //     Arrange
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);
            var paymentMethodId = await CreatePaymentMethodAsync();
            var paymentId = await CreatePaymentAsync(studentId, invoiceId, paymentMethodId);
            var refund = new Payment
            {
                InvoiceId = invoiceId,
                RefundedPaymentId = paymentId,
                IsActive = true,
                Amount = -1000,
                StudentId = studentId,
                PaymentMethodId = paymentMethodId,
                RefundDate = DateTimeOffset.UtcNow,
                PaymentDate = DateTimeOffset.UtcNow
            };
            var refundId = await _paymentRepository.AddAsync(refund);
            Assert.IsGreaterThan(0, refundId);
            // Act  
            var refundPaymentData = await _paymentRepository.GetRefundPaymentDataByPaymentId(paymentId);
            // Assert
            Assert.IsNotNull(refundPaymentData);
            Assert.IsGreaterThan(0, refundPaymentData.RefundedPaymentId.Value);

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


        private async Task<int> CreatePaymentAsync(int studentId, int invoiceId, int paymentMethodId)
        {
            var payment = new PaymentBuilder()
                .WithStudentId(studentId).WithInvoiceId(invoiceId)
                .WithPaymentMethodId(paymentMethodId).WithAmount(1000)
                .Build();

            return await _paymentRepository.AddAsync(payment);
        }

        private async Task<int> CreatePaymentMethodAsync()
        {
            var paymentMethod = new PaymentMethod
            {
                PaymentMethodType = PaymentMethodType.Cash,
                Name = "Method 1",
                IsActive = true
            };
            return await _paymentMethodRepository.AddAsync(paymentMethod);
        }
        #endregion

    }
}
