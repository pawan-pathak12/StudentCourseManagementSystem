using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.Refunds
{
    [TestClass]
    public class RefundServiceValidateEligibilityTests : RefundServiceBaseClass
    {
        [TestMethod]
        public async Task ValidateEligibility_ShouldReturnTrue_WhenAllRulesPass()
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
                .WithPaymentMethodId(paymentMethodId).WithAmount(1000)
                .Build();
            var paymentId = await _paymentRepository.AddAsync(payment);

            //Act 
            var (success, errorMessage) = await _refundService.ValidateEligibilityAsync(paymentId);

            //Assert
            Assert.IsTrue(success);
        }

        [TestMethod]
        public async Task ValidateEligibilityAsync_ShouldReturnFalse_WhenPaymentNotFound()
        {
            //Arrange 

            //Act 

            //Assert
        }

        [TestMethod]
        public async Task ValidateEligibilityAsync_ShouldReturnFalse_WhenPaymentStatusIsNotCompleted()
        {
            //Arrange 

            //Act 

            //Assert
        }

        [TestMethod]
        public async Task ValidateEligibilityAsync_ShouldReturnFalse_WhenPaymentIsAlreadyRefunded()
        {
            //Arrange 

            //Act 

            //Assert
        }

        [TestMethod]
        public async Task ValidateEligibilityAsync_ShouldReturnFalse_WhenRefundWindowHasExpired()
        {
            //Arrange 

            //Act 

            //Assert
        }

        [TestMethod]
        public async Task ValidateEligibilityAsync_ShouldReturnFalse_WhenCourseStartedTooLongAgo()
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
                .WithPaymentMethodId(paymentMethodId).WithAmount(1000)
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
