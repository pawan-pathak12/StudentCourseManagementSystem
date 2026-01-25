using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.Payments
{
    [TestClass]
    public class ProcessPayment : PaymentServiceTestBase
    {
        #region Phase 4 : Payment Processing Tests 
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
            var (feeTemplateId, feeTemplateAmount) = await CreateFeeTemplateAsync(courseId); //clculation type : Flat amount 

            var feeAssessment = new FeeAssessmentBuilder()
                .WithCourseId(courseId).WithEnrollmentId(enrollmentId)
                .WithFeeTemplateId(feeTemplateId).WithFeeAssessmentStatus(AssessmentStatus.Assessed)
                .WithAmount(feeTemplateAmount).Build();
            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new InvoiceBuilder()
                .WithCourseId(courseId).WithStudentId(studentId)
                .WithFeeAssessmentId(feeAssessmentId).WithBalanceDue(feeAssessment.Amount)
                .WithInvoiceStatus(InvoiceStatus.Cancelled).WithTotalAmount(feeAssessment.Amount)
                .Build();
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
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();

            var enrollmentId = await _enrollmentRepository.AddAsync(new Enrollment { StudentId = studentId, CourseId = courseId, IsActive = true });
            var (feeTemplateId, feeTemplateAmount) = await CreateFeeTemplateAsync(courseId); //clculation type : Flat amount 

            var feeAssessment = new FeeAssessmentBuilder()
                        .WithCourseId(courseId).WithEnrollmentId(enrollmentId)
                        .WithFeeTemplateId(feeTemplateId).WithFeeAssessmentStatus(AssessmentStatus.Assessed)
                        .WithAmount(feeTemplateAmount).Build();
            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new InvoiceBuilder()
                       .WithCourseId(courseId).WithStudentId(studentId)
                       .WithFeeAssessmentId(feeAssessmentId).WithBalanceDue(feeAssessment.Amount)
                       .WithInvoiceStatus(InvoiceStatus.Cancelled).WithTotalAmount(feeAssessment.Amount)
                       .Build();
            var invoiceId = await _invoiceRepository.AddAsync(invoice);

            var paymentMethodId = 111111;
            decimal amountPaid = 1000;

            //Act 
            var (sucess, errorMessage) = await _paymentService.ProcessPaymentAsync(invoiceId, paymentMethodId, amountPaid);

            //Assert 
            Assert.IsFalse(sucess);
            Assert.IsNotNull(errorMessage);
        }

        [TestMethod]
        public async Task ProcessPaymentAsync_IfEnterAmountIsNeagtiveOrZero_ReturnsFalse()
        {
            //Arrange
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();

            var enrollmentId = await _enrollmentRepository.AddAsync(new Enrollment { StudentId = studentId, CourseId = courseId, IsActive = true });
            var (feeTemplateId, feeTemplateAmount) = await CreateFeeTemplateAsync(courseId); //clculation type : Flat amount 

            var feeAssessment = new FeeAssessmentBuilder()
                           .WithCourseId(courseId).WithEnrollmentId(enrollmentId)
                           .WithFeeTemplateId(feeTemplateId).WithFeeAssessmentStatus(AssessmentStatus.Assessed)
                           .WithAmount(feeTemplateAmount).Build();
            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new InvoiceBuilder()
                         .WithCourseId(courseId).WithStudentId(studentId)
                         .WithFeeAssessmentId(feeAssessmentId).WithBalanceDue(feeAssessment.Amount)
                         .WithInvoiceStatus(InvoiceStatus.Issued).WithTotalAmount(feeAssessment.Amount)
                         .Build();
            var invoiceId = await _invoiceRepository.AddAsync(invoice);

            var paymentMethodId = await CreatePaymentMethodAsync();
            decimal amountPaid = -1000;

            //Act 
            var (sucess, errorMessage) = await _paymentService.ProcessPaymentAsync(invoiceId, paymentMethodId, amountPaid);

            //Assert 
            Assert.IsFalse(sucess);
            Assert.IsNotNull(errorMessage);
        }

        [TestMethod]
        public async Task ProcessPaymentAsync_PaidAmountIsGreaterThenBalanceDue_ReturnsFalse()
        {

            //Arrange
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();

            var enrollmentId = await _enrollmentRepository.AddAsync(new Enrollment { StudentId = studentId, CourseId = courseId, IsActive = true });
            var (feeTemplateId, feeTemplateAmount) = await CreateFeeTemplateAsync(courseId); //clculation type : Flat amount 

            var feeAssessment = new FeeAssessmentBuilder()
                         .WithCourseId(courseId).WithEnrollmentId(enrollmentId)
                         .WithFeeTemplateId(feeTemplateId).WithFeeAssessmentStatus(AssessmentStatus.Assessed)
                         .WithAmount(feeTemplateAmount).Build();
            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new InvoiceBuilder()
                         .WithCourseId(courseId).WithStudentId(studentId)
                         .WithFeeAssessmentId(feeAssessmentId).WithBalanceDue(feeAssessment.Amount)
                         .WithInvoiceStatus(InvoiceStatus.Issued).WithTotalAmount(feeAssessment.Amount)
                         .Build();
            var invoiceId = await _invoiceRepository.AddAsync(invoice);

            var paymentMethodId = await CreatePaymentMethodAsync();
            decimal amountPaid = 111000;

            //Act 
            var (sucess, errorMessage) = await _paymentService.ProcessPaymentAsync(invoiceId, paymentMethodId, amountPaid);

            //Assert 
            Assert.IsFalse(sucess);
            Assert.IsNotNull(errorMessage);
        }
        [TestMethod]
        public async Task ProcessPayment_PartialPayment_UpdatesInvoiceCorrectly()
        {

            #region Arrange
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();

            var enrollmentId = await _enrollmentRepository.AddAsync(new Enrollment { StudentId = studentId, CourseId = courseId, IsActive = true });
            var (feeTemplateId, feeTemplateAmount) = await CreateFeeTemplateAsync(courseId); //clculation type : Flat amount 

            var feeAssessment = new FeeAssessmentBuilder()
                        .WithCourseId(courseId).WithEnrollmentId(enrollmentId)
                        .WithFeeTemplateId(feeTemplateId).WithFeeAssessmentStatus(AssessmentStatus.Assessed)
                        .WithAmount(feeTemplateAmount).Build();
            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new InvoiceBuilder()
                         .WithCourseId(courseId).WithStudentId(studentId)
                         .WithFeeAssessmentId(feeAssessmentId).WithBalanceDue(feeAssessment.Amount)
                         .WithInvoiceStatus(InvoiceStatus.Issued).WithTotalAmount(feeAssessment.Amount)
                         .Build();
            var invoiceId = await _invoiceRepository.AddAsync(invoice);

            var paymentMethodId = await CreatePaymentMethodAsync();
            decimal amountPaid = 100;

            #endregion

            //Act 
            var (sucess, errorMessage) = await _paymentService.ProcessPaymentAsync(invoiceId, paymentMethodId, amountPaid);

            //Assert 
            Assert.IsTrue(sucess);
            var amount = invoice.AmountPaid + amountPaid;
            var invoiceData = await _invoiceRepository.GetByIdAsync(invoiceId);
            Assert.IsNotNull(invoiceData);

            var balanceDue = invoice.TotalAmount - invoice.AmountPaid;
            Assert.AreEqual(amountPaid, invoiceData.AmountPaid);
            Assert.AreEqual(balanceDue, invoiceData.BalanceDue);

        }

        [TestMethod]
        public async Task ProcessPayment_FullPayment_UpdatesInvoiceCorrectly()
        {
            #region Arrange
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();

            var enrollmentId = await _enrollmentRepository.AddAsync(new Enrollment { StudentId = studentId, CourseId = courseId, IsActive = true });
            var (feeTemplateId, feeTemplateAmount) = await CreateFeeTemplateAsync(courseId); //clculation type : Flat amount 

            var feeAssessment = new FeeAssessmentBuilder()
                          .WithCourseId(courseId).WithEnrollmentId(enrollmentId)
                          .WithFeeTemplateId(feeTemplateId).WithFeeAssessmentStatus(AssessmentStatus.Assessed)
                          .WithAmount(feeTemplateAmount).Build();
            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new InvoiceBuilder()
                         .WithCourseId(courseId).WithStudentId(studentId)
                         .WithFeeAssessmentId(feeAssessmentId).WithBalanceDue(feeAssessment.Amount)
                         .WithInvoiceStatus(InvoiceStatus.Issued).WithTotalAmount(feeAssessment.Amount)
                         .Build();
            var invoiceId = await _invoiceRepository.AddAsync(invoice);

            var paymentMethodId = await CreatePaymentMethodAsync();
            decimal amountPaid = invoice.TotalAmount;

            #endregion

            //Act 
            var (sucess, errorMessage) = await _paymentService.ProcessPaymentAsync(invoiceId, paymentMethodId, amountPaid);

            //Assert 
            Assert.IsTrue(sucess);
            var amount = invoice.AmountPaid + amountPaid;
            var invoiceData = await _invoiceRepository.GetByIdAsync(invoiceId);
            Assert.IsNotNull(invoiceData);

            var balanceDue = invoice.TotalAmount - invoice.AmountPaid;
            Assert.AreEqual(amountPaid, invoiceData.AmountPaid);
            Assert.AreEqual(balanceDue, invoiceData.BalanceDue);
        }
        [TestMethod]
        public async Task ProcessPaymentAsync_IfFullyPaid_UpdateFeeAssessmentSuccessfully()
        {
            #region Arrange
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();

            var enrollmentId = await _enrollmentRepository.AddAsync(new Enrollment { StudentId = studentId, CourseId = courseId, IsActive = true });
            var (feeTemplateId, feeTemplateAmount) = await CreateFeeTemplateAsync(courseId); //clculation type : Flat amount 

            var feeAssessment = new FeeAssessmentBuilder()
                        .WithCourseId(courseId).WithEnrollmentId(enrollmentId)
                        .WithFeeTemplateId(feeTemplateId).WithFeeAssessmentStatus(AssessmentStatus.Assessed)
                        .WithAmount(feeTemplateAmount).Build();
            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new InvoiceBuilder()
                         .WithCourseId(courseId).WithStudentId(studentId)
                         .WithFeeAssessmentId(feeAssessmentId).WithBalanceDue(feeAssessment.Amount)
                         .WithInvoiceStatus(InvoiceStatus.Issued).WithTotalAmount(feeAssessment.Amount)
                         .Build();
            var invoiceId = await _invoiceRepository.AddAsync(invoice);

            var paymentMethodId = await CreatePaymentMethodAsync();
            decimal amountPaid = invoice.TotalAmount;

            #endregion

            //Act 
            var (sucess, errorMessage) = await _paymentService.ProcessPaymentAsync(invoiceId, paymentMethodId, amountPaid);

            //Assert 
            Assert.IsTrue(sucess);

            var feeAssessmentData = await _feeAssessmentRepository.GetByInvoiceIdAsync(invoiceId);
            Assert.IsNotNull(feeAssessmentData);
            Assert.AreEqual(AssessmentStatus.Paid, feeAssessmentData.FeeAssessmentStatus);


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

        private async Task<(int, decimal)> CreateFeeTemplateAsync(int courseId)
        {
            var feeTemplate = new FeeTemplateBuilder()
                .WithCourseId(courseId).WithAmount(4000).
                WithCalculationType(CalculationType.FlatAmount).Build();

            var feeTemplateId = await _feeTemplateRepository.AddAsync(feeTemplate);
            return (feeTemplateId, feeTemplate.Amount);

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
