using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

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

            var feeAssessment = new FeeAssessment
            {
                CourseId = courseId,
                EnrollmentId = enrollmentId,
                FeeTemplateId = feeTemplateId,
                IsActive = true,
                FeeAssessmentStatus = AssessmentStatus.Assessed,
                Amount = feeTemplateAmount,
                DueDate = DateTimeOffset.UtcNow.AddDays(10),
            };

            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new Invoice
            {
                CourseId = courseId,
                StudentId = studentId,
                FeeAssessmentId = feeAssessmentId,
                BalanceDue = feeAssessment.Amount,
                AmountPaid = 0,
                InvoiceNumber = $"INV-.....",
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                InvoiceStatus = InvoiceStatus.Cancelled,
                TotalAmount = feeAssessment.Amount,
                DueDate = feeAssessment.DueDate,
            };
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

            var feeAssessment = new FeeAssessment
            {
                CourseId = courseId,
                EnrollmentId = enrollmentId,
                FeeTemplateId = feeTemplateId,
                IsActive = true,
                FeeAssessmentStatus = AssessmentStatus.Assessed,
                Amount = feeTemplateAmount,
                DueDate = DateTimeOffset.UtcNow.AddDays(10),
            };
            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);
            var invoice = new Invoice
            {
                CourseId = courseId,
                StudentId = studentId,
                FeeAssessmentId = feeAssessmentId,
                BalanceDue = feeAssessment.Amount,
                AmountPaid = 0,
                InvoiceNumber = $"INV-.....",
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                InvoiceStatus = InvoiceStatus.Issued,
                TotalAmount = feeAssessment.Amount,
                DueDate = feeAssessment.DueDate,
            };
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

            var feeAssessment = new FeeAssessment
            {
                CourseId = courseId,
                EnrollmentId = enrollmentId,
                FeeTemplateId = feeTemplateId,
                IsActive = true,
                FeeAssessmentStatus = AssessmentStatus.Assessed,
                Amount = feeTemplateAmount,
                DueDate = DateTimeOffset.UtcNow.AddDays(10),
            };

            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new Invoice
            {
                CourseId = courseId,
                StudentId = studentId,
                FeeAssessmentId = feeAssessmentId,
                BalanceDue = feeAssessment.Amount,
                AmountPaid = 0,
                InvoiceNumber = $"INV-.....",
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                InvoiceStatus = InvoiceStatus.Issued,
                TotalAmount = feeAssessment.Amount,
                DueDate = feeAssessment.DueDate,
            };
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

            var feeAssessment = new FeeAssessment
            {
                CourseId = courseId,
                EnrollmentId = enrollmentId,
                FeeTemplateId = feeTemplateId,
                IsActive = true,
                FeeAssessmentStatus = AssessmentStatus.Assessed,
                Amount = feeTemplateAmount,
                DueDate = DateTimeOffset.UtcNow.AddDays(10),
            };

            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new Invoice
            {
                CourseId = courseId,
                StudentId = studentId,
                FeeAssessmentId = feeAssessmentId,
                BalanceDue = feeAssessment.Amount,
                AmountPaid = 0,
                InvoiceNumber = $"INV-.....",
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                InvoiceStatus = InvoiceStatus.Issued,
                TotalAmount = feeAssessment.Amount,
                DueDate = feeAssessment.DueDate,
            };
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

            var feeAssessment = new FeeAssessment
            {
                CourseId = courseId,
                EnrollmentId = enrollmentId,
                FeeTemplateId = feeTemplateId,
                IsActive = true,
                FeeAssessmentStatus = AssessmentStatus.Assessed,
                Amount = feeTemplateAmount,
                DueDate = DateTimeOffset.UtcNow.AddDays(10),
            };

            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new Invoice
            {
                CourseId = courseId,
                StudentId = studentId,
                FeeAssessmentId = feeAssessmentId,
                BalanceDue = feeAssessment.Amount,
                AmountPaid = 0,
                InvoiceNumber = $"INV-.....",
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                InvoiceStatus = InvoiceStatus.Issued,
                TotalAmount = feeAssessment.Amount,
                DueDate = feeAssessment.DueDate,
            };
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

            var feeAssessment = new FeeAssessment
            {
                CourseId = courseId,
                EnrollmentId = enrollmentId,
                FeeTemplateId = feeTemplateId,
                IsActive = true,
                FeeAssessmentStatus = AssessmentStatus.Assessed,
                Amount = feeTemplateAmount,
                DueDate = DateTimeOffset.UtcNow.AddDays(10),
            };

            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new Invoice
            {
                CourseId = courseId,
                StudentId = studentId,
                FeeAssessmentId = feeAssessmentId,
                BalanceDue = feeAssessment.Amount,
                AmountPaid = 0,
                InvoiceNumber = $"INV-.....",
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                InvoiceStatus = InvoiceStatus.Issued,
                TotalAmount = feeAssessment.Amount,
                DueDate = feeAssessment.DueDate,
            };
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

            var feeAssessment = new FeeAssessment
            {
                CourseId = courseId,
                EnrollmentId = enrollmentId,
                FeeTemplateId = feeTemplateId,
                IsActive = true,
                FeeAssessmentStatus = AssessmentStatus.Assessed,
                Amount = feeTemplateAmount,
                DueDate = DateTimeOffset.UtcNow.AddDays(10),
            };

            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new Invoice
            {
                CourseId = courseId,
                StudentId = studentId,
                FeeAssessmentId = feeAssessmentId,
                BalanceDue = feeAssessment.Amount,
                AmountPaid = 0,
                InvoiceNumber = $"INV-.....",
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                InvoiceStatus = InvoiceStatus.Issued,
                TotalAmount = feeAssessment.Amount,
                DueDate = feeAssessment.DueDate,
            };
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
            var course = new Course
            {
                Code = "A112A",
                Title = "Introduction to Computer Science",
                Credits = 3,
                Description = "Basic concepts of programming, algorithms, and problem-solving.",
                Instructor = "Dr. Sharma",
                StartDate = DateTimeOffset.UtcNow.AddDays(25),
                EndDate = DateTimeOffset.UtcNow.AddMonths(2),
                IsActive = true,
                Capacity = 20,
                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(5),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(15)
            };

            return await _courseRepository.AddAsync(course);
        }
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

        private async Task<(int, decimal)> CreateFeeTemplateAsync(int courseId)
        {
            var feeTemplate = new FeeTemplate
            {
                CourseId = courseId,
                CalculationType = CalculationType.FlatAmount,
                Amount = 2000,
                IsActive = true,
                Name = "Lab fee",
                RatePerCredit = 0
            };

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
