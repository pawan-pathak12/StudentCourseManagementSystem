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

        #endregion


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

        #region CURD Operations 
        [TestMethod]
        public async Task AddAsync_WithValid_InsertsRowAndReturnsId()
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

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
            Assert.IsTrue(paymentId > 0);
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


        #region Helper Method

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
                CourseId = courseId
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
                UpdatedAt = null
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
                TotalAmount = 0,
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
                StudentId = studentId,              // link to Student (e.g., Sita Sharma)
                InvoiceId = invoiceId,           // link to Invoice (e.g., INV-2026-001)
                PaymentMethodId = paymentMethodId,        // e.g., 1 = Cash, 2 = Bank Transfer, 3 = Card
                IsActive = true,
                Amount = 7500.00m,          // partial payment
                PaymentDate = new DateTimeOffset(2026, 01, 25, 14, 30, 0, TimeSpan.FromHours(5.75)),
                ReferenceNumber = "TXN-2026-ABC123",
                Notes = "First installment of tuition fee",
                ProcessedBy = "AdminUser01",
                CreatedDate = DateTimeOffset.UtcNow
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
