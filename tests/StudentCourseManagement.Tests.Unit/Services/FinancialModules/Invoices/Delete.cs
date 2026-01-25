using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.Invoices
{
    [TestClass]
    public class Delete : InvocieServiceTestBaseClass
    {
        [TestMethod]
        public async Task DeleteAsync_WithExistingId_ReturnTrue()
        {
            //Arrange 
            var invoice = new InvoiceBuilder()
     .WithCourseId(1).WithFeeAssessmentId(1).WithStudentId(1)
     .WithInvoiceStatus(InvoiceStatus.Issued).Build();
            var invoiceId = await _invoiceRepository.AddAsync(invoice);

            //Act 
            var result = await _invoiceService.DeleteAsync(invoiceId);

            //Assert
            Assert.IsTrue(result);
            var invoiceData = await _invoiceRepository.GetByIdAsync(invoiceId);
            Assert.IsNull(invoiceData);

        }

        [TestMethod]
        public async Task DeleteAsync_WithNonExistingId_ReturnTrue()
        {
            int invoiceId = 1111111;

            //Act 
            var result = await _invoiceService.DeleteAsync(invoiceId);

            //Assert
            Assert.IsFalse(result);


        }
    }
}
