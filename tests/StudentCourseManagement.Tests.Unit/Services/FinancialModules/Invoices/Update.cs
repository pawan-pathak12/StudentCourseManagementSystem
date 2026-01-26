using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Common.Builders;
using StudentCourseManagement.Tests.Common.Builders.FinancialModule;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.Invoices
{
    [TestClass]
    public class Update : InvocieServiceTestBaseClass
    {
        [TestMethod]
        public async Task UpdateAsync_WithValildData_ReturnsTrue()
        {
            //Arrange 
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var feeAssessmentId = await CreateFeeAssessmentAsync(1, courseId, studentId);

            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);

            var updateData = new InvoiceBuilder()
                  .WithStudentId(studentId).WithCourseId(courseId).WithInvoiceId(invoiceId)
                 .WithFeeAssessmentId(feeAssessmentId).WithInvoiceStatus(InvoiceStatus.PartiallyPaid)
                 .WithUpdatedAt(DateTimeOffset.UtcNow).Build();

            //Act
            var result = await _invoiceService.UpdateAsync(invoiceId, updateData);

            //Assert 
            Assert.IsTrue(result);
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
            Assert.AreEqual(updateData.InvoiceStatus, invoice?.InvoiceStatus);
            Assert.AreEqual(updateData.UpdatedAt, invoice?.UpdatedAt);
        }
        [TestMethod]
        public async Task UpdateAsync_IfInvoiceIdMissing_ReturnsFalse()
        {

            //Arrange 
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var feeAssessmentId = await CreateFeeAssessmentAsync(1, courseId, studentId);

            var invoiceId = await CreateInvoiceAsync(studentId, courseId, feeAssessmentId);

            var updateData = new Invoice
            {
                AmountPaid = 1000,
                InvoiceStatus = InvoiceStatus.PartiallyPaid,
                IsActive = true
            };

            //Act
            var result = await _invoiceService.UpdateAsync(invoiceId, updateData);

            //Assert 
            Assert.IsFalse(result);

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
