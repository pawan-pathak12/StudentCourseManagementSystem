using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.Payments
{
    [TestClass]
    public class Create : PaymentServiceTestBase
    {
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
                Discount = 0
            };
            return await _invoiceRepository.AddAsync(invoice);
        }

        private async Task<int> CreatePaymentMethodAsync()
        {
            var method = new PaymentMethod
            {
                PaymentMethodType = PaymentMethodType.Cash,
                Provider = "TestProvider",
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
