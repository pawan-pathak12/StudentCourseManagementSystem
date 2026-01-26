using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Common.Builders;
using StudentCourseManagement.Tests.Common.Builders.FinancialModule;
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

            var updatedPayment = new PaymentBuilder()
                .WithPaymentId(paymentId).WithStudentId(studentId)
                .WithInvoiceId(invoiceId).WithPaymentMethodId(paymentMethodId).WithAmount(20000.0m)
                .WithNotes("Updated Notes").WithProcessedBy("Admin User")
                .Build();
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
            //Arrange
            var paymentId = 99999;
            var updatedPayment = new PaymentBuilder()
                .WithPaymentId(paymentId).WithStudentId(1)
                .WithInvoiceId(1).WithPaymentMethodId(1).WithAmount(20000.0m)
                .WithNotes("Updated Notes").WithProcessedBy("Admin User")
                .Build();
            // Act
            var result = await _paymentService.UpdateAsync(paymentId, updatedPayment);

            // Assert
            Assert.IsFalse(result);
        }



        #region Private Helper Methods
        private async Task<int> CreateStudentAsync()
        {
            var student = new StudentBuilder()
               .Build();
            return await _studentRepository.AddAsync(student);
        }

        private async Task<int> CreateInvoiceAsync(int studentId)
        {
            var invoice = new InvoiceBuilder()
                .WithStudentId(studentId).Build();
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
            var payment = new PaymentBuilder()
                 .WithStudentId(studentId).WithInvoiceId(invoiceId)
                 .WithPaymentMethodId(paymentMethodId).WithAmount(15000)
                 .WithPaymentStatus(PaymentStatus.Completed).Build();

            return await _paymentRepository.AddAsync(payment);
        }
        #endregion
    }
}
