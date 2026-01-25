using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.Invoices
{
    [TestClass]
    public class Create : InvocieServiceTestBaseClass
    {
        [TestMethod]
        public async Task CreateAsync_WithValidData_ReturnsTrue()
        {
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var feeAssessmentId = await CreateFeeAssessmentAsync(1, courseId, studentId);

            var invoice = new InvoiceBuilder()
                .WithStudentId(studentId).WithCourseId(courseId).WithFeeAssessmentId(feeAssessmentId)
                .WithIssuedAt(DateTimeOffset.UtcNow).Build();
            var (sucess, errorMessage, invoiceId) = await _invoiceService.CreateAsync(invoice);

            Assert.IsTrue(sucess);

        }
        [TestMethod]
        public async Task CreateAsync_IfCourseIdMissing_ReturnsFalse()
        {
            //Arrange 
            var studentId = await CreateStudentAsync();

            var feeAssessment = new FeeAssessmentBuilder()
               .WithAmount(15000.00m).WithFeeAssessmentStatus(AssessmentStatus.Pending)
               .Build();
            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new InvoiceBuilder()
                .WithStudentId(studentId).WithFeeAssessmentId(feeAssessmentId)
                .WithInvoiceStatus(InvoiceStatus.Issued).Build();

            //Act 
            var (sucess, errorMessage, invoiceId) = await _invoiceService.CreateAsync(invoice);
            //result
            Assert.IsFalse(sucess);
        }
        [TestMethod]
        public async Task CreateAsync_IfStudentIdMissing_ReturnsFalse()
        {
            //Arrange 
            var courseId = await CreateCourseAsync();
            var feeAssessment = new FeeAssessmentBuilder()
             .WithAmount(15000.00m).WithFeeAssessmentStatus(AssessmentStatus.Pending)
          .WithCourseId(courseId).Build();

            var feeAssessmentId = await _feeAssessmentRepository.AddAsync(feeAssessment);

            var invoice = new InvoiceBuilder()
               .WithCourseId(courseId).WithFeeAssessmentId(feeAssessmentId)
               .WithInvoiceStatus(InvoiceStatus.Issued).Build();

            //Act 
            var (sucess, errorMessage, invoiceId) = await _invoiceService.CreateAsync(invoice);
            //result
            Assert.IsFalse(sucess);
            Assert.IsNotNull(errorMessage);

        }
        [TestMethod]
        public async Task CreateAsync_IfFeeAssessmentIdMissing_ReturnsFalse()
        {
            //Arrange 
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();

            var invoice = new InvoiceBuilder()
             .WithCourseId(courseId).WithStudentId(studentId)
             .WithInvoiceStatus(InvoiceStatus.Issued).Build();
            //Act 
            var (sucess, errorMessage, invoiceId) = await _invoiceService.CreateAsync(invoice);
            //result
            Assert.IsFalse(sucess);
            Assert.IsNotNull(errorMessage);

        }

        #region Private Helper Methods
        private async Task<int> CreateStudentAsync()
        {
            var student = new StudentBuilder()
                .Build();
            return await _studentRepository.AddAsync(student);
        }

        private async Task<int> CreateCourseAsync()
        {
            var course = new CourseBuilder()
                .WithInstructor("Dr. Anil Sharma").WithDescription("Fundamentals of programming using C# and .NET Core.")
                .WithTitle("Introduction to Programming").Build();

            return await _courseRepository.AddAsync(course);
        }
        private async Task<int> CreateFeeAssessmentAsync(int enrollmentid, int courseId, int feeTemplateId)
        {
            var feeAssessment = new FeeAssessmentBuilder()
                .WithEnrollmentId(enrollmentid).WithCourseId(courseId).WithFeeTemplateId(feeTemplateId)
                .WithAmount(1500.00m).WithFeeAssessmentStatus(AssessmentStatus.Assessed)
                .Build();
            return await _feeAssessmentRepository.AddAsync(feeAssessment);
        }
        private async Task<int> CreateInvoiceAsync(int studentId, int courseId, int feeAssessmentId)
        {

            var invoice = new InvoiceBuilder()
                .WithStudentId(studentId).WithCourseId(courseId).WithFeeAssessmentId(feeAssessmentId)
                .Build();
            return await _invoiceRepository.AddAsync(invoice);
        }

        #endregion

    }
}
