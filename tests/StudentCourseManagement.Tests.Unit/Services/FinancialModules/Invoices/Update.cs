using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Entities.FinancialModule;
using StudentCourseManagement.Domain.Enums;
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

            var updateData = new Invoice
            {
                InvoiceId = invoiceId,
                AmountPaid = 1000,
                InvoiceStatus = InvoiceStatus.PartiallyPaid,
                IsActive = true,
                UpdatedAt = DateTimeOffset.UtcNow
            };

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
            var student = new Student
            {
                Name = "Sita Sharma",
                Email = "sita.sharma@example.com",
                DOB = new DateTimeOffset(2004, 05, 12, 0, 0, 0, TimeSpan.FromHours(5.75)),
                Number = 9812345678,
                IsActive = true,
                Gender = "Female",
                Address = "Biratnagar, Nepal"
            };
            return await _studentRepository.AddAsync(student);
        }

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
        private async Task<int> CreateFeeAssessmentAsync(int enrollmentid, int courseId, int FeeTemplateId)
        {
            var feeAssessment = new FeeAssessment
            {
                EnrollmentId = enrollmentid,
                CourseId = courseId,
                FeeTemplateId = FeeTemplateId,
                Amount = 15000.00m,
                DueDate = DateTime.UtcNow.AddDays(30),
                FeeAssessmentStatus = AssessmentStatus.Pending,
                IsActive = true,
                PaidDate = null,
                LateFeeAmount = null,
                LateFeeAppliedDate = null
            };
            return await _feeAssessmentRepository.AddAsync(feeAssessment);
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
