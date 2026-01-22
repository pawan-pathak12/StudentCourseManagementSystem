using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.Payments
{
    [TestClass]
    public class Delete : PaymentServiceTestBase
    {
        [TestMethod]
        public async Task DeleteAsync_WithExistingPaymentId_ReturnsTrue()
        {
            //arrange 
            var payment = new Payment
            {
                StudentId = 1,
                InvoiceId = 1,
                PaymentMethodId = 1,
                IsActive = true,
                Amount = 15000.00m,
                PaymentDate = DateTimeOffset.UtcNow,
                PaymentStatus = PaymentStatus.Completed,
                ReferenceNumber = "TXN-2026-ABC123",
                Notes = "Initial payment",
                ProcessedBy = "TestUser",
                CreatedDate = DateTimeOffset.UtcNow
            };


            var paymentId = await _paymentRepository.AddAsync(payment);

            //act 
            var result = await _paymentService.DeleteAsync(paymentId);
            //assert 
            Assert.IsTrue(result);
            var paymentData = await _paymentRepository.GetByIdAsync(paymentId);
            Assert.IsNull(paymentData);


        }
    }
}
