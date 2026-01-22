using StudentCourseManagement.Domain.Entities.FinancialModule;
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
            var invoiceLineItem = new InvoiceLineItem
            {
                CourseId = 1,
                FeeTemplateId = 1,
                InvoiceId = 1,
                Amount = 1000,
                IsActive = true,
                Description = "testing"
            };
            var invoiceLineItemId = await _invoiceLineItemRepository.AddAsync(invoiceLineItem);

            var update = new InvoiceLineItem
            {
                InvoiceLineItemId = invoiceLineItemId,
                InvoiceId = invoiceLineItem.InvoiceId,
                CourseId = invoiceLineItem.CourseId,
                Description = "Update Testing",
                IsActive = true
            };


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
            var update = new InvoiceLineItem
            {
                InvoiceId = 1,
                CourseId = 1,
                Description = "Update Testing",
                IsActive = true
            };


            //Act
            var result = await _invoiceLineItemService.UpdateAsync(1, update);

            //Assert
            Assert.IsFalse(result);
        }

        #region Private Helper Metohds 

        #endregion
    }
}
