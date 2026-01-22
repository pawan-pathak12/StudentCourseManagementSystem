using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.Payments
{
    [TestClass]
    public class Update : PaymentServiceTestBase
    {
        [TestMethod]
        public async Task UpdateAsync_WithExistingId_ReturnsTrue_AndUpdatesData()
        {
            // Arrange
            var studentId = await CreateStudentAsync();
            var invoiceId = await CreateInvoiceAsync(studentId);
            var paymentMethodId = await CreatePaymentMethodAsync();
            var paymentId = await CreatePaymentAsync(studentId, invoiceId, paymentMethodId);

            var updatedPayment = new Payment
            {
                PaymentId = paymentId,
                StudentId = studentId,
                InvoiceId = invoiceId,
                PaymentMethodId = paymentMethodId,
                IsActive = true,
                Amount = 20000.00m,
                PaymentDate = DateTimeOffset.UtcNow.AddDays(1),
                PaymentStatus = PaymentStatus.Completed,
                ReferenceNumber = "TXN-2026-UPDATED",
                Notes = "Updated notes",
                ProcessedBy = "AdminUser",
                CreatedDate = DateTimeOffset.UtcNow
            };

            // Act
            var result = await _paymentService.UpdateAsync(paymentId, updatedPayment);

            // Assert
            Assert.IsTrue(result);

            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            Assert.IsNotNull(payment);
            Assert.AreEqual(updatedPayment.Notes, payment?.Notes);
            Assert.AreEqual(updatedPayment.ProcessedBy, payment?.ProcessedBy);
        }
        [TestMethod]
        public async Task UpdateAsync_WhenNonExistingPaymentId_ReturnsFalse()
        {

            var paymentId = 99999;
            var updatedPayment = new Payment
            {
                PaymentId = paymentId,
                StudentId = 1,
                InvoiceId = 1,
                PaymentMethodId = 1,
                IsActive = true,
                Amount = 20000.00m,
                PaymentDate = DateTimeOffset.UtcNow.AddDays(1),
                PaymentStatus = PaymentStatus.Completed,
                ReferenceNumber = "TXN-2026-UPDATED",
                Notes = "Updated notes",
                ProcessedBy = "AdminUser",
                CreatedDate = DateTimeOffset.UtcNow
            };

            // Act
            var result = await _paymentService.UpdateAsync(paymentId, updatedPayment);

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
                Name = "Cash",
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
