using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Common.Builders;
using StudentCourseManagement.Tests.Common.Builders.FinancialModule;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.Refunds
{
    [TestClass]
    public class ProcessRefundTests : RefundServiceBaseClass
    {
        private async Task<int> CreateDataAsync()
        {
            //Arrange 
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();

            var enrollment = new EnrollmentBuilder()
                .WithStudentId(studentId).WithCourseId(courseId)
                .Build();
            var enrollmentId = await _enrollmentRepository.AddAsync(enrollment);

            var feeTemplate = new FeeTemplateBuilder()
                .WithCourseId(courseId).WithAmount(1000)
                .WithCalculationType(CalculationType.FlatAmount).Build();
            var feeTemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);

            var feeAssessment = new FeeAssessmentBuilder()
                .WithEnrollmentId(enrollmentId).WithCourseId(courseId)
                .WithFeeTemplateId(feeTemplateId).Build();
            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new InvoiceBuilder()
                .WithCourseId(courseId).WithFeeAssessmentId(feeAssessmentId)
                .WithStudentId(studentId).Build();
            var invoiceId = await _invoiceRepository.AddAsync(invoice);

            var paymentMethodId = await CreatePaymentMethodAsync();

            var payment = new PaymentBuilder()
                .WithStudentId(studentId).WithInvoiceId(invoiceId)
                .WithPaymentMethodId(paymentMethodId).WithAmount(100).WithPaymentStatus(PaymentStatus.Completed)
                .Build();
            return await _paymentRepository.AddAsync(payment);
        }
        //process refund return tru  
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldReturnTrue_WhenItPassesAllRules()
        {
            //Arrange
            var paymentId = await CreateDataAsync();
            //Act 
            var (success, errorMessage) = await _refundService.ProcessRefundAsync(paymentId, "Testing");
            //Assert
            Assert.IsTrue(success);
            Assert.IsNull(errorMessage);

        }

        //add refund payment 
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldCreateNegativePayment()
        {
            //Arrange
            var paymentId = await CreateDataAsync();
            //Act 
            var (success, errorMessage) = await _refundService.ProcessRefundAsync(paymentId, "Testing");

            //Assert
            Assert.IsTrue(success);
            var refundPayment = await _paymentRepository.GetRefundPaymentDataByPaymentId(paymentId);
            Assert.IsNotNull(refundPayment);
            Assert.IsNegative(refundPayment.Amount);

        }

        //update payment status 
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldUpdateOrginalPaymentStatus()
        {
            //Arrange
            var paymentId = await CreateDataAsync();
            //Act 
            var (success, errorMessage) = await _refundService.ProcessRefundAsync(paymentId, "Testing");
            //Assert
            Assert.IsTrue(success);
            var paymentData = await _paymentRepository.GetByIdAsync(paymentId);
            Assert.IsNotNull(paymentData);
            Assert.AreEqual(PaymentStatus.Refunded, paymentData.PaymentStatus);
        }

        //update invocie 
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldUpdatesInvoiceBalance()
        {
            //Arrange 
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();

            var enrollment = new EnrollmentBuilder()
                .WithStudentId(studentId).WithCourseId(courseId)
                .Build();
            var enrollmentId = await _enrollmentRepository.AddAsync(enrollment);

            var feeTemplate = new FeeTemplateBuilder()
                .WithCourseId(courseId).WithAmount(1000)
                .WithCalculationType(CalculationType.FlatAmount).Build();
            var feeTemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);

            var feeAssessment = new FeeAssessmentBuilder()
                .WithEnrollmentId(enrollmentId).WithCourseId(courseId)
                .WithFeeTemplateId(feeTemplateId).Build();
            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var paidAmount = 1000;
            var invoice = new InvoiceBuilder()
                .WithCourseId(courseId).WithFeeAssessmentId(feeAssessmentId)
                .WithTotalAmount(feeTemplate.Amount).WithBalanceDue(0)
                .WithAmountPaid(paidAmount)
              .WithStudentId(studentId).Build();
            var invoiceId = await _invoiceRepository.AddAsync(invoice);

            var paymentMethodId = await CreatePaymentMethodAsync();

            var payment = new PaymentBuilder()
                .WithStudentId(studentId).WithInvoiceId(invoiceId)
                .WithPaymentMethodId(paymentMethodId).WithAmount(paidAmount).WithPaymentStatus(PaymentStatus.Completed)
                .Build();
            var paymentId = await _paymentRepository.AddAsync(payment);
            //Act 
            var (success, errorMessage) = await _refundService.ProcessRefundAsync(paymentId, "Testing");
            //Assert
            Assert.IsTrue(success);
            var invoiceAfterUpdate = await _invoiceRepository.GetByIdAsync(invoiceId);
            Assert.IsNotNull(invoiceAfterUpdate);
            Assert.AreEqual(0, invoiceAfterUpdate.AmountPaid);
            Assert.AreEqual(invoice.BalanceDue, invoiceAfterUpdate.BalanceDue);


        }

        // update feeass
        [TestMethod]
        public async Task ProcessRefundAsync_ShouldUpdatesfeeAssessment_WhenInvoiceWasPaid()
        {
            //Arrange 
            //Arrange 
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();

            var enrollment = new EnrollmentBuilder()
                .WithStudentId(studentId).WithCourseId(courseId)
                .Build();
            var enrollmentId = await _enrollmentRepository.AddAsync(enrollment);

            var feeTemplate = new FeeTemplateBuilder()
                .WithCourseId(courseId).WithAmount(1000)
                .WithCalculationType(CalculationType.FlatAmount).Build();
            var feeTemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);

            var paidAmount = 1000;

            var feeAssessment = new FeeAssessmentBuilder()
                .WithEnrollmentId(enrollmentId).WithCourseId(courseId)
                .WithAmount(paidAmount).WithPaidDate(DateTimeOffset.UtcNow)
                .WithFeeTemplateId(feeTemplateId).Build();

            var paidDate = feeAssessment.PaidDate;
            var asseStatus = feeAssessment.FeeAssessmentStatus;

            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new InvoiceBuilder()
                .WithCourseId(courseId).WithFeeAssessmentId(feeAssessmentId)
                .WithTotalAmount(feeTemplate.Amount).WithBalanceDue(0)
                .WithAmountPaid(paidAmount).WithInvoiceStatus(InvoiceStatus.Paid)
              .WithStudentId(studentId).Build();
            var invoiceId = await _invoiceRepository.AddAsync(invoice);

            var paymentMethodId = await CreatePaymentMethodAsync();

            var payment = new PaymentBuilder()
                .WithStudentId(studentId).WithInvoiceId(invoiceId)
                .WithPaymentMethodId(paymentMethodId).WithAmount(paidAmount).WithPaymentStatus(PaymentStatus.Completed)
                .Build();
            var paymentId = await _paymentRepository.AddAsync(payment);

            var (success, errorMessage) = await _refundService.ProcessRefundAsync(paymentId, "Testing");

            //Assert
            Assert.IsTrue(success);
            var updatedFeeAssessment = await _feeAssessmentRepository.GetByIdAsync(feeAssessmentId);
            Assert.IsNotNull(updatedFeeAssessment);
            Assert.AreEqual(asseStatus, updatedFeeAssessment.FeeAssessmentStatus);
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
        #endregion

    }
}
