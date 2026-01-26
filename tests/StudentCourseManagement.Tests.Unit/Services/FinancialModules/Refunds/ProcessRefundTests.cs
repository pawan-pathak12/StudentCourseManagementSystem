using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.Refunds
{
    [TestClass]
    public class ProcessRefundTests : RefundServiceBaseClass
    {
        //process refund return tru  
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldReturnTrue_WhenItPassesAllRules()
        {
            //Arrange 

            //Act 

            //Assert

        }

        // valid logic 
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldPass_ValidEligibility()
        {
            //Arrange 

            //Act 

            //Assert
        }
        //add refund pamynet 
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldCreateNegativePayment()
        {
            //Arrange 

            //Act 

            //Assert
        }

        //update payment status 
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldUpdateOrginalPaymentStatus()
        {
            //Arrange 

            //Act 

            //Assert
        }

        //update invocie 
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldUpdatesInvoiceBalance()
        {
            //Arrange 

            //Act 

            //Assert
        }

        // update feeass
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldUpdatesfeeAssessment_WhenInvoiceWasPaid()
        {
            //Arrange 

            //Act 

            //Assert
        }

        #region Helper Moethods 
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
