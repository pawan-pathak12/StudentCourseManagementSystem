using StudentCourseManagement.Tests.Common.Builders.FinancialModule;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.InvoiceLineItems
{
    [TestClass]
    public class Update : InvoiceLineItemServiceTestBaseClass
    {
        [TestMethod]
        public async Task UpdateAsync_WithValidData_ReturnsTrue()
        {
            //Arrange
            var invoiceLineItem = new InvoiceLineItemBuilder()
                   .WithCourseId(1).WithInvoiceId(1).WithAmount(1000)
                  .WithFeeTemplateId(1).Build();

            var invoiceLineItemId = await _invoiceLineItemRepository.AddAsync(invoiceLineItem);

            var update = new InvoiceLineItemBuilder()
                 .WithCourseId(invoiceLineItem.CourseId).WithInvoiceLineItemId(invoiceLineItemId)
                 .WithInvoiceId(invoiceLineItem.InvoiceId).WithAmount(1000)
                .WithFeeTemplateId(invoiceLineItem.FeeTemplateId).
                WithDescription("Update Testing").Build();
            //Act
            var result = await _invoiceLineItemService.UpdateAsync(invoiceLineItemId, update);

            //Assert
            Assert.IsTrue(result);

            var lineItem = await _invoiceLineItemRepository.GetByIdAsync(invoiceLineItemId);
            Assert.AreEqual(update.Description, lineItem?.Description);

        }
        [TestMethod]
        public async Task UpdateAsync_IfInvoicelineItemIdMissing_Returnsfalse()
        {
            var invoiceLineItem = new InvoiceLineItemBuilder()
           .WithCourseId(1).WithInvoiceId(1).WithAmount(1000)
          .WithFeeTemplateId(1).Build();
            //Act
            var result = await _invoiceLineItemService.UpdateAsync(1, invoiceLineItem);

            //Assert
            Assert.IsFalse(result);
        }

    }
}
