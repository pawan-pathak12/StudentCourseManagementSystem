using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Unit.Common.FInacialModules;

namespace StudentCourseManagement.Tests.Unit.Services.FinancialModules.InvoiceLineItems
{
    [TestClass]
    public class Create : InvoiceLineItemServiceTestBaseClass
    {
        [TestMethod]
        public async Task CreateAsync_WithValidData_ReturnTrue()
        {
            var courseId = await CreateCourseAsync();
            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var invoiceId = await CreateInvoiceAsync(1, courseId, 1);
            var lineItem = new InvoiceLineItem
            {
                CourseId = courseId,
                InvoiceId = invoiceId,
                FeeTemplateId = feeTemplateId,
                CreatedAt = DateTimeOffset.UtcNow,
                Amount = 1333,
                IsActive = true
            };

            //act 
            var (success, errorMessage, linetemId) = await _invoiceLineItemService.CreateAsync(lineItem);

            //assert
            Assert.IsTrue(success);
        }
        [TestMethod]
        public async Task CreateAsync_WhenCourseIdMissing_ReturnFalse()
        {
            //arrange 
            var courseId = await CreateCourseAsync();

            var feeTemplateId = await CreateFeeTemplateAsync(courseId);
            var invoiceId = await CreateInvoiceAsync(1, courseId, 1);
            var lineItem = new InvoiceLineItem
            {
                InvoiceId = invoiceId,
                FeeTemplateId = feeTemplateId,
                CreatedAt = DateTimeOffset.UtcNow,
                Amount = 1333
            };

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
            var lineItem = new InvoiceLineItem
            {
                CourseId = courseId,
                InvoiceId = invoiceId,
                CreatedAt = DateTimeOffset.UtcNow,
                Amount = 1333
            };

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
            var lineItem = new InvoiceLineItem
            {
                CourseId = courseId,
                FeeTemplateId = feeTemplateId,
                CreatedAt = DateTimeOffset.UtcNow,
                Amount = 1333
            };

            //act 
            var (success, errorMessage, linetemId) = await _invoiceLineItemService.CreateAsync(lineItem);

            //assert
            Assert.IsFalse(success);
        }

        #region private helper methods
        private async Task<int> CreateCourseAsync()
        {
            var course = new Course
            {
                Code = "CS1001",
                Title = "Introduction to Programming",
                Credits = 3,
                Description = "Fundamentals of programming using C# and .NET Core.",
                Instructor = "Dr. Anil Sharma",
                StartDate = DateTimeOffset.UtcNow.AddDays(40),
                EndDate = DateTimeOffset.UtcNow.AddMonths(2),
                IsActive = true,
                Capacity = 50,
                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(10),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(25),
            };

            return await _courseRepository.AddAsync(course);
        }
        private async Task<int> CreateFeeTemplateAsync(int courseId)
        {
            var feeTemplate = new FeeTemplate
            {
                CourseId = courseId,
                CreatedAt = DateTimeOffset.UtcNow,
                Name = "c# basic",
                IsActive = true
            };

            return await _feeTemplateRepository.AddAsync(feeTemplate);
        }
        private async Task<int> CreateInvoiceAsync(int studentId, int courseId, int feeAssessmentId)
        {
            var invoice = new Invoice
            {
                InvoiceId = 9001,
                InvoiceNumber = "INV-2026-001",
                StudentId = studentId,
                CourseId = courseId,
                IsActive = true,
                FeeAssessmentId = feeAssessmentId,
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
            return await _invoiceRepository.AddAsync(invoice);
        }
        #endregion
    }
}
