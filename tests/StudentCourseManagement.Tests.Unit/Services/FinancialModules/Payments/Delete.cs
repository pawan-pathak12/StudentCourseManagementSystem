using StudentCourseManagement.Tests.Unit.Common.FInacialModules;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.Payments
{
    [TestClass]
    public class Delete : PaymentServiceTestBase
    {
        [TestMethod]
        public async Task DeleteAsync_WithExistingPaymentId_ReturnsTrue()
        {
            //arrange 
            var payment = new PaymentBuilder()
                .WithStudentId(1).WithInvoiceId(1)
                .WithPaymentMethodId(1).WithAmount(15000.0m)
                .WithProcessedBy("Test User").Build();



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
