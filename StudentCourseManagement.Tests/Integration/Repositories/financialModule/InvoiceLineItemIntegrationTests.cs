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
    public class InvoiceLineItemIntegrationTests
    {
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;
        private readonly EnrollmentRepository _enrollmentRepository;
        private readonly FeeAssessmentRepository _feeAssessmentRepository;
        private readonly InvoiceRepository _invoiceRepository;
        private readonly FeeTemplateRepository _feeTemplate;
        private readonly InvoiceLineItemRepository _invoiceLineItemRepository;

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
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);

            //Act 
            var invoiceLineItemId = await CreateInvoiceLineItemAsync(invoiceId, courseId, feeTemplateId);

            //Arrange 
            Assert.IsTrue(invoiceLineItemId > 0);

        }

        [TestMethod]
        public async Task GettAllAsync_IfNotNullThen_ReturnListOfInvoiceLineItem()
        {
            //Arrange 
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

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
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

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
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var feeAssessmentId = await CreateFeeAssessmentAsync(enrollmentId, courseId, feeTemplateId);
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);
            var invoiceLineItemId = await CreateInvoiceLineItemAsync(invoiceId, courseId, feeTemplateId);


            var updated = new InvoiceLineItem
            {
                InvoiceLineItemId = invoiceLineItemId,
                InvoiceId = invoiceId,
                FeeTemplateId = feeTemplateId,
                CourseId = courseId,
                Description = "Updated description",
                Quantity = 2,
                UnitPrice = 1200m,
                Amount = 2400m
            };

            //Act 
            var result = await _invoiceLineItemRepository.UpdateAsync(invoiceLineItemId, updated);

            //Assert
            // Assert
            Assert.IsTrue(result);

            var fromDb = await _invoiceLineItemRepository.GetByIdAsync(invoiceLineItemId);
            Assert.AreEqual("Updated description", fromDb?.Description);
            Assert.AreEqual(2, fromDb?.Quantity);
            Assert.AreEqual(2400m, fromDb?.Amount);


        }

        [TestMethod]
        public async Task DeleteAsync_WithActiveId_SetsIsActiveFalse()
        {
            //Arrange 
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

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
        private async Task<int> CreateInvoiceLineItemAsync(int invoiceId, int courseId, int feeTemplateId)
        {
            var invoiceLineItem = new InvoiceLineItem
            {

                CourseId = courseId,
                FeeTemplateId = feeTemplateId,
                InvoiceId = invoiceId,
                IsActive = true,
                Description = "Tuition fee per credit ",
                Quantity = 3,              // number of credits
                UnitPrice = 2500.00m,      // fee per credit
                Amount = 3 * 2500.00m,     // total = Quantity × UnitPrice = 7500
                CreatedAt = DateTimeOffset.UtcNow
            };
            return await _invoiceLineItemRepository.AddAsync(invoiceLineItem);

        }
        #endregion
    }
}
