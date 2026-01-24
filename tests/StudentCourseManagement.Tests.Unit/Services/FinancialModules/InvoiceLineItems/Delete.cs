using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentCourseManagement.Domain.Entities.FinancialModule;
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

            var lineItem = new InvoiceLineItem
            {
                CourseId = 1,
                InvoiceId = 1,
                FeeTemplateId = 1,
                CreatedAt = DateTimeOffset.UtcNow,
                Amount = 1333,
                IsActive = true
            };

            var lineItemId = await _invoiceLineItemRepository.AddAsync(lineItem);
            //Act
            var result = await _invoiceLineItemService.DeleteAsync(lineItemId);
            //Assert
            Assert.IsTrue(result);
        }

    }
}
