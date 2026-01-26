using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Common.Builders;
using StudentCourseManagement.Tests.Common.Builders.FinancialModule;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.InvoiceLineItems
{
    [TestClass]
    public class Create : InvoiceLineItemServiceTestBaseClass
    {
        [TestMethod]
        public async Task CreateAsync_WithValidData_ReturnTrue()
        {
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var invoiceId = await CreateInvoiceAsync(studentId, courseId, 1);

            var lineItem = new InvoiceLineItemBuilder()
                .WithCourseId(courseId).WithInvoiceId(invoiceId).WithAmount(1000)
               .WithFeeTemplateId(feeTemplateId).Build();

            //act 
            var (success, errorMessage, linetemId) = await _invoiceLineItemService.CreateAsync(lineItem);

            //assert
            Assert.IsTrue(success);
            Assert.IsGreaterThan(0, linetemId);
        }
        [TestMethod]
        public async Task CreateAsync_WhenCourseIdMissing_ReturnFalse()
        {
            //arrange 
            var courseId = await CreateCourseAsync();

            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var invoiceId = await CreateInvoiceAsync(1, courseId, 1);
            var lineItem = new InvoiceLineItemBuilder()
               .WithFeeTemplateId(feeTemplateId).WithInvoiceId(invoiceId).WithAmount(1000)
                 .Build();
            //act 
            var (success, errorMessage, linetemId) = await _invoiceLineItemService.CreateAsync(lineItem);

            //assert
            Assert.IsFalse(success);
        }

        [TestMethod]
        public async Task CreateAsync_WhenFeeTemplateIdIdMissing_ReturnFalse()
        {
            var courseId = await CreateCourseAsync();
            var invoiceId = await CreateInvoiceAsync(1, courseId, 1);
            var lineItem = new InvoiceLineItemBuilder()
               .WithCourseId(courseId).WithInvoiceId(invoiceId).WithAmount(1000)
               .Build();

            //act 
            var (success, errorMessage, linetemId) = await _invoiceLineItemService.CreateAsync(lineItem);

            //assert
            Assert.IsFalse(success);
        }

        [TestMethod]
        public async Task CreateAsync_InvoiceIdMissing_ReturnFalse()
        {
            var courseId = await CreateCourseAsync();
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var lineItem = new InvoiceLineItemBuilder()
                 .WithCourseId(courseId).WithFeeTemplateId(feeTemplateId).WithAmount(1000)
                 .Build();

            //act 
            var (success, errorMessage, linetemId) = await _invoiceLineItemService.CreateAsync(lineItem);

            //assert
            Assert.IsFalse(success);
        }

        #region private helper methods
        private async Task<int> CreateStudentAsync()
        {
            var student = new StudentBuilder()
            .Build();
            return await _studentRepository.AddAsync(student);
        }
        private async Task<int> CreateCourseAsync()
        {
            var course = new CourseBuilder()
                .Build();
            return await _courseRepository.AddAsync(course);
        }
        private async Task<int> CreateFeeTemplateAsync(int courseId)
        {
            var feeTemplate = new FeeTemplateBuilder()
                .WithCourseId(courseId).WithAmount(4000).
                WithRatePerCredit(100).Build();

            return await _feeTemplateRepository.AddAsync(feeTemplate);
        }
        private async Task<int> CreateInvoiceAsync(int studentId, int courseId, int feeAssessmentId)
        {
            var invoice = new InvoiceBuilder()
                .WithInvoiceNumber($"INV-{DateTime.UtcNow:yyyyymmdd}").WithStudentId(studentId)
                .WithCourseId(courseId).WithFeeAssessmentId(feeAssessmentId).WithAmountPaid(0)
                .WithBalanceDue(0).WithInvoiceStatus(InvoiceStatus.Issued).Build();

            return await _invoiceRepository.AddAsync(invoice);
        }
        #endregion
    }
}
