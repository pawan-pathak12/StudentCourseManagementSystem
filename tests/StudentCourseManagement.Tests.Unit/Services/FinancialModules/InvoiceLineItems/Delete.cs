using StudentCourseManagement.Tests.Common.Builders.FinancialModule;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.InvoiceLineItems
{
    [TestClass]
    public class Delete : InvoiceLineItemServiceTestBaseClass
    {
        [TestMethod]
        public async Task DeleteAsyncWithExistingId_ReturnsTrue()
        {
            //Arrange 

            var lineItem = new InvoiceLineItemBuilder()
                .WithCourseId(1).WithInvoiceId(1).WithAmount(1000)
              .WithFeeTemplateId(1).Build();

            var lineItemId = await _invoiceLineItemRepository.AddAsync(lineItem);
            //Act
            var result = await _invoiceLineItemService.DeleteAsync(lineItemId);
            //Assert
            Assert.IsTrue(result);
        }

    }
}
