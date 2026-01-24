using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.Invoices
{
    [TestClass]
    public class Delete : InvocieServiceTestBaseClass
    {
        [TestMethod]
        public async Task DeleteAsync_WithExistingId_ReturnTrue()
        {
            //Arrange 
            var invoice = new Invoice
            {
                InvoiceId = 9001,
                InvoiceNumber = "INV-2026-001",
                StudentId = 1,
                CourseId = 1,
                IsActive = true,
                FeeAssessmentId = 1,
                LateFeeApplied = false,
                IssuedAt = new DateTimeOffset(2026, 01, 20, 10, 0, 0, TimeSpan.FromHours(5.75)),
                DueDate = DateTimeOffset.UtcNow.AddDays(30),
                TotalAmount = 0,
                InvoiceStatus = InvoiceStatus.Issued,
                CreatedAt = DateTimeOffset.UtcNow,
                AmountPaid = 0,
                BalanceDue = 0,
                UpdatedAt = DateTimeOffset.UtcNow,
                Discount = 0
            };
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
