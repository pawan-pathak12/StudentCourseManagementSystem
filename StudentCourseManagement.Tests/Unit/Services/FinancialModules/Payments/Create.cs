using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.Payments
{
    [TestClass]
    public class Create : PaymentServiceTestBase
    {
        #region Manually Payment tests 
        [TestMethod]
        public async Task CreateAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var studentId = await CreateStudentAsync();
            var invoiceId = await CreateInvoiceAsync(studentId);
            var paymentMethodId = await CreatePaymentMethodAsync();

            var payment = new Payment
            {
                StudentId = studentId,
                InvoiceId = invoiceId,
                PaymentMethodId = paymentMethodId,
                IsActive = true,
                Amount = 15000.00m,
                PaymentDate = DateTimeOffset.UtcNow,
                PaymentStatus = PaymentStatus.Completed,
                ReferenceNumber = "TXN-2026-ABC123",
                Notes = "Initial payment",
                ProcessedBy = "TestUser",
                CreatedDate = DateTimeOffset.UtcNow
            };

            // Act
            var result = await _paymentService.CreateAsync(payment);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CreateAsync_IfStudentIdMissing_ReturnsFalse()
        {
            // Arrange
            var paymentMethodId = await CreatePaymentMethodAsync();

            var payment = new Payment
            {
                InvoiceId = 1,
                PaymentMethodId = paymentMethodId,
                IsActive = true,
                Amount = 15000.00m,
                PaymentDate = DateTimeOffset.UtcNow,
                PaymentStatus = PaymentStatus.Completed,
                ReferenceNumber = "TXN-2026-ABC123",
                Notes = "Initial payment",
                ProcessedBy = "TestUser",
                CreatedDate = DateTimeOffset.UtcNow
            };

            // Act
            var result = await _paymentService.CreateAsync(payment);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CreateAsync_IfPInvoiceIdMissing_ReturnsFalse()
        {
            // Arrange
            var studentId = await CreateStudentAsync();
            var paymentMethodId = await CreatePaymentMethodAsync();

            var payment = new Payment
            {
                StudentId = studentId,
                PaymentMethodId = paymentMethodId,
                IsActive = true,
                Amount = 15000.00m,
                PaymentDate = DateTimeOffset.UtcNow,
                PaymentStatus = PaymentStatus.Completed,
                ReferenceNumber = "TXN-2026-ABC123",
                Notes = "Initial payment",
                ProcessedBy = "TestUser",
                CreatedDate = DateTimeOffset.UtcNow
            };

            // Act
            var result = await _paymentService.CreateAsync(payment);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CreateAsync_IfPaymentMethodIdMissing_ReturnsFalse()
        {
            // Arrange
            var studentId = await CreateStudentAsync();
            var invoiceId = await CreateInvoiceAsync(studentId);

            var payment = new Payment
            {
                StudentId = studentId,
                InvoiceId = invoiceId,
                IsActive = true,
                Amount = 15000.00m,
                PaymentDate = DateTimeOffset.UtcNow,
                PaymentStatus = PaymentStatus.Completed,
                ReferenceNumber = "TXN-2026-ABC123",
                Notes = "Initial payment",
                ProcessedBy = "TestUser",
                CreatedDate = DateTimeOffset.UtcNow
            };

            // Act
            var result = await _paymentService.CreateAsync(payment);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region Phase 4 : Payment Processing Tests 
        [TestMethod]
        public async Task ProcessPaymentAsync_WithValidData_ReturnsTrue()
        {
            //Arrange
            int studentId = 1;
            var invoiceId = await CreateInvoiceAsync(studentId);
            var paymentMethodId = await CreatePaymentMethodAsync();
            var amountPaid = 1000;
            //Act 
            var (success, errorMessage) = await _paymentService.ProcessPaymentAsync(invoiceId, paymentMethodId, amountPaid);

            //Assert 
            Assert.IsTrue(success);
            Assert.IsNull(errorMessage);
        }

        [TestMethod]
        public async Task ProcessPaymentAsync_WithNoExistingInvoiceId_ReturnsFalse()
        {
            //Arrange
            int invoiceId = 999999;
            var paymentMethodId = await CreatePaymentMethodAsync();
            var amountPaid = 1000;
            //Act 
            var (success, errorMessage) = await _paymentService.ProcessPaymentAsync(invoiceId, paymentMethodId, amountPaid);

            //Assert 
            Assert.IsFalse(success);
            Assert.IsNotNull(errorMessage);

        }

        [TestMethod]
        public async Task ProcessPaymentAsync_IfInvoiceIsNotPayable_ReturnsFalse()
        {
            //Arrange
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();

            var enrollmentId = await _enrollmentRepository.AddAsync(new Enrollment { StudentId = studentId, CourseId = courseId, IsActive = true });
            var feeTemplate = new FeeTemplate
            {
                CourseId = courseId,
                CalculationType = CalculationType.FlatAmount,
                Amount = 2000,
                IsActive = true,
                Name = "Lab fee",
                RatePerCredit = 0
            };

            var feeTemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);

            var feeAssessment = new FeeAssessment
            {
                CourseId = courseId,
                EnrollmentId = enrollmentId,
                FeeTemplateId = feeTemplateId,
                IsActive = true,
                FeeAssessmentStatus = AssessmentStatus.Assessed,
                Amount = feeTemplate.Amount,
                DueDate = DateTimeOffset.UtcNow.AddDays(10),
            };

            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new Invoice
            {
                CourseId = courseId,
                StudentId = studentId,
                FeeAssessmentId = feeAssessmentId,
                BalanceDue = feeAssessment.Amount,
                AmountPaid = 0,
                InvoiceNumber = $"INV-.....",
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                InvoiceStatus = InvoiceStatus.Cancelled,
                TotalAmount = feeTemplate.Amount,
                DueDate = feeAssessment.DueDate,
            };
            var invoiceId = await _invoiceRepository.AddAsync(invoice);

            var paymentMethodId = await CreatePaymentMethodAsync();
            decimal amountPaid = 1000;

            //Act 
            var (sucess, errorMessage) = await _paymentService.ProcessPaymentAsync(invoiceId, paymentMethodId, amountPaid);

            //Assert 
            Assert.IsFalse(sucess);
            Assert.IsNotNull(errorMessage);
        }
        [TestMethod]
        public async Task ProcessPaymentAsync_IfPaymentMethodNotFound_ReturnsFalse()
        {
            //Arrange

            //Act 

            //Assert 
        }
        [TestMethod]
        public async Task ProcessPaymentAsync_IdPaymentMethodIsInActive_ReturnsFalse()
        {
            //Arrange

            //Act 

            //Assert 
        }

        [TestMethod]
        public async Task ProcessPaymentAsync_IfEnterAmountIsNeagtiveOrZero_ReturnsFalse()
        {
            //Arrange

            //Act 

            //Assert 
        }

        [TestMethod]
        public async Task ProcessPaymentAsync_PaidAmountIsGreaterThenBalanceDue_ReturnsFalse()
        {
            //Arrange

            //Act 

            //Assert 
        }
        [TestMethod]
        public async Task ProcessPayment_PartialPayment_UpdatesInvoiceCorrectly()
        {
            //Arrange

            //Act 

            //Assert 
        }

        [TestMethod]
        public async Task ProcessPayment_FullPayment_UpdatesInvoiceCorrectly()
        {
            //Arrange

            //Act 

            //Assert 
        }
        [TestMethod]
        public async Task ProcessPaymentAsync_IfFullyPaid_UpdateFeeAssessmentSuccessfully()
        {
            //Arrange

            //Act 

            //Assert 
        }
        #endregion

        #region Private Helper Methods
        private async Task<int> CreateCourseAsync()
        {
            var course = new Course
            {
                Code = "A112A",
                Title = "Introduction to Computer Science",
                Credits = 3,
                Description = "Basic concepts of programming, algorithms, and problem-solving.",
                Instructor = "Dr. Sharma",
                StartDate = DateTimeOffset.UtcNow.AddDays(25),
                EndDate = DateTimeOffset.UtcNow.AddMonths(2),
                IsActive = true,
                Capacity = 20,
                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(5),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(15)
            };

            return await _courseRepository.AddAsync(course);
        }
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

        private async Task<int> CreateInvoiceAsync(int studentId)
        {
            var invoice = new Invoice
            {
                InvoiceId = 0,
                InvoiceNumber = "INV-2026-001",
                StudentId = studentId,
                IsActive = true,
                LateFeeApplied = false,
                IssuedAt = DateTimeOffset.UtcNow,
                DueDate = DateTimeOffset.UtcNow.AddDays(30),
                TotalAmount = 0,
                InvoiceStatus = StudentCourseManagement.Domain.Enums.InvoiceStatus.Issued,
                CreatedAt = DateTimeOffset.UtcNow,
                AmountPaid = 0,
                BalanceDue = 0,
                UpdatedAt = DateTimeOffset.UtcNow,
                Discount = 0,
            };
            return await _invoiceRepository.AddAsync(invoice);
        }

        private async Task<int> CreatePaymentMethodAsync()
        {
            var method = new PaymentMethod
            {
                PaymentMethodType = PaymentMethodType.Cash,
                Name = "TestName",
                IsActive = true
            };
            return await _paymentMethodRepository.AddAsync(method);
        }

        private async Task<int> CreatePaymentAsync(int studentId, int invoiceId, int paymentMethodId)
        {
            var payment = new Payment
            {
                StudentId = studentId,
                InvoiceId = invoiceId,
                PaymentMethodId = paymentMethodId,
                IsActive = true,
                Amount = 15000.00m,
                PaymentDate = DateTimeOffset.UtcNow,
                PaymentStatus = PaymentStatus.Completed,
                ReferenceNumber = "TXN-INIT",
                Notes = "Init",
                ProcessedBy = "Test",
                CreatedDate = DateTimeOffset.UtcNow
            };
            return await _paymentRepository.AddAsync(payment);
        }
        #endregion
    }
}
