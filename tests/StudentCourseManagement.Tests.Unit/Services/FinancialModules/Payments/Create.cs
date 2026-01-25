using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders.FinancialModule;

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

            var payment = new PaymentBuilder()
                .WithStudentId(studentId).WithInvoiceId(invoiceId)
                .WithPaymentMethodId(paymentMethodId).WithAmount(15000.0m)
                .WithProcessedBy("Test User").Build();


            // Act
            var (sucess, errorMessage, paymentId) = await _paymentService.CreateAsync(payment);

            // Assert
            Assert.IsTrue(sucess);
            Assert.IsNull(errorMessage);
        }

        [TestMethod]
        public async Task CreateAsync_IfStudentIdMissing_ReturnsFalse()
        {
            // Arrange
            var paymentMethodId = await CreatePaymentMethodAsync();

            var payment = new PaymentBuilder()
                 .WithInvoiceId(1)
                  .WithPaymentMethodId(paymentMethodId).WithAmount(15000.0m)
                  .WithProcessedBy("Test User").Build();

            // Act
            var (sucess, errorMessage, paymentId) = await _paymentService.CreateAsync(payment);

            // Assert
            Assert.IsFalse(sucess);
        }

        [TestMethod]
        public async Task CreateAsync_IfPInvoiceIdMissing_ReturnsFalse()
        {
            // Arrange
            var studentId = await CreateStudentAsync();
            var paymentMethodId = await CreatePaymentMethodAsync();

            var payment = new PaymentBuilder()
                .WithStudentId(studentId)
                .WithPaymentMethodId(paymentMethodId).WithAmount(15000.0m)
                .WithProcessedBy("Test User").Build();

            // Act
            var (sucess, errorMessage, paymentId) = await _paymentService.CreateAsync(payment);

            // Assert
            Assert.IsFalse(sucess);
        }

        [TestMethod]
        public async Task CreateAsync_IfPaymentMethodIdMissing_ReturnsFalse()
        {
            // Arrange
            var studentId = await CreateStudentAsync();
            var invoiceId = await CreateInvoiceAsync(studentId);

            var payment = new PaymentBuilder()
                .WithStudentId(studentId).WithInvoiceId(invoiceId)
                .WithAmount(15000.0m)
                .WithProcessedBy("Test User").Build();


            // Act
            var (sucess, errorMessage, paymentId) = await _paymentService.CreateAsync(payment);

            // Assert
            Assert.IsFalse(sucess);
        }

        #endregion

        #region Private Helper Methods
        private async Task<int> CreateCourseAsync()
        {
            var course = new CourseBuilder()
                .WithInstructor("Dr. Anil Sharma").WithDescription("Fundamentals of programming using C# and .NET Core.")
                .WithTitle("Introduction to Programming").Build();

            return await _courseRepository.AddAsync(course);
        }
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
                Name = "TestName",
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
