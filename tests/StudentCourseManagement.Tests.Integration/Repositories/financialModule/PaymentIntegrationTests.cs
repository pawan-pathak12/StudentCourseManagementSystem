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

            var updatedPayment = new Payment
            {
                PaymentId = paymentId,
                StudentId = studentId,
                InvoiceId = invoiceId,
                PaymentMethodId = paymentMethodId,
                IsActive = true,
                Amount = 15000.00m,
                PaymentDate = DateTimeOffset.UtcNow.AddDays(1),
                PaymentStatus = PaymentStatus.Completed,
                ReferenceNumber = "TXN-2026-XYZ789",
                Notes = "Final installment, invoice fully settled",
                ProcessedBy = "AdminUser02",
                CreatedDate = DateTimeOffset.UtcNow
            };
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
            var student = new Student
            {
                Name = "Sita Sharma 1",
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
                Title = "Introduction to Programming 1",
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
                Amount = 1000
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
                Amount = 1000.00m,
                DueDate = DateTime.UtcNow.AddDays(30),
                FeeAssessmentStatus = AssessmentStatus.Pending,
                IsActive = true,
                PaidDate = null,
                LateFeeAmount = null,
                LateFeeAppliedDate = null
            };
            return await _feeAssessmentRepository.AddAsync(feeAssessment);
        }
        private async Task<int> CreateInvoiceAsync(int studentId, int courseId, int feeAssessmentId)
        {
            var invoice = new Invoice
            {
                InvoiceId = 9001,
                InvoiceNumber = "INV-2026-001",
                StudentId = studentId,
                CourseId = courseId,
                IsActive = true,
                FeeAssessmentId = feeAssessmentId,
                LateFeeApplied = false,
                IssuedAt = new DateTimeOffset(2026, 01, 20, 10, 0, 0, TimeSpan.FromHours(5.75)),
                DueDate = DateTimeOffset.UtcNow.AddDays(30),
                TotalAmount = 1000,
                InvoiceStatus = InvoiceStatus.Issued,
                CreatedAt = DateTimeOffset.UtcNow,
                AmountPaid = 0,
                BalanceDue = 0,
                UpdatedAt = DateTimeOffset.UtcNow,
                Discount = 0
            };
            return await _invoiceRepository.AddAsync(invoice);
        }

        private async Task<int> CreatePaymentAsync(int studentId, int invoiceId, int paymentMethodId)
        {
            var payment = new Payment
            {
                StudentId = studentId,
                InvoiceId = invoiceId,
                PaymentMethodId = paymentMethodId,
                IsActive = true,
                Amount = 1000.00m,
                PaymentDate = new DateTimeOffset(2026, 01, 25, 14, 30, 0, TimeSpan.FromHours(5.75)),
                ReferenceNumber = "TXN-2026-ABC123",
                Notes = "First installment of tuition fee",
                ProcessedBy = "AdminUser01",
                CreatedDate = DateTimeOffset.UtcNow,
                RefundedPaymentId = null

            };
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
