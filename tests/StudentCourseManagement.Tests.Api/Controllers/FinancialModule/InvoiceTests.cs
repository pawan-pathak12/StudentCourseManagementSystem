using StudentCourseManagement.Application.DTOs.FInancialModule.Invoices;
using StudentCourseManagement.Domain.Constants;
using StudentCourseManagement.Domain.Enums;
using System.Net;
using System.Net.Http.Json;

namespace StudentCourseManagement.Tests.Api.Controllers.FinancialModule
{
    [TestClass]
    public class InvoiceTests : IntegrationTestBase
    {

        #region Happy Path 

        [TestMethod]
        public async Task Create_WhenValid_Return201()
        {
            //Arrange
            var student = await builder.CreateAndReturnStudent();
            var course = await builder.CreateAndReturnCourse();
            var feeTemplate = await builder.CreateAndReturnFeeTemplate(course!.CourseId);
            var enrollment = await builder.CreateAndReturnEnrollment(student!.StudentId, course!.CourseId);
            var feeAssessment = await builder.CreateAndReturnFeeAssessment(enrollment!.EnrollmentId, feeTemplate!.FeeTemplateId);

            var invoice = new CreateInvoiceDto
            {
                StudentId = student!.StudentId,
                CourseId = course!.CourseId,
                DueDate = DateTimeOffset.UtcNow.AddDays(FinancialConstants.DUE_DATE_DAYS),
                FeeAssessmentId = feeAssessment!.FeeAssessmentId,
                feeTemplateId = feeTemplate!.FeeTemplateId
            };

            //Act 
            var response = await _client.PostAsJsonAsync("/api/invoice", invoice);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public async Task GetAll_WhenInvoiceExists_Return200()
        {
            //Arrange
            var student = await builder.CreateAndReturnStudent();
            var course = await builder.CreateAndReturnCourse();
            var feeTemplate = await builder.CreateAndReturnFeeTemplate(course!.CourseId);
            var enrollment = await builder.CreateAndReturnEnrollment(student!.StudentId, course!.CourseId);
            var feeAssessment = await builder.CreateAndReturnFeeAssessment(enrollment!.EnrollmentId, feeTemplate!.FeeTemplateId);
            var invoice = await builder.CreateAndReturnInvoice(student!.StudentId, feeAssessment!.FeeAssessmentId, course!.CourseId);

            //Act 
            var response = await _client.GetAsync("/api/invoice");
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
        [TestMethod]
        public async Task GetById_WhenInvoiceExists_Return201()
        {
            //Arrange
            var student = await builder.CreateAndReturnStudent();
            var course = await builder.CreateAndReturnCourse();
            var feeTemplate = await builder.CreateAndReturnFeeTemplate(course!.CourseId);
            var enrollment = await builder.CreateAndReturnEnrollment(student!.StudentId, course!.CourseId);
            var feeAssessment = await builder.CreateAndReturnFeeAssessment(enrollment!.EnrollmentId, feeTemplate!.FeeTemplateId);
            var invoice = await builder.CreateAndReturnInvoice(student!.StudentId, feeAssessment!.FeeAssessmentId, course!.CourseId);
            //Act 
            var response = await _client.GetAsync($"/api/invoice/{invoice!.InvoiceId}");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }

        [TestMethod]
        public async Task Update_WhenValid_Return200()
        {
            //Arrange
            var student = await builder.CreateAndReturnStudent();
            var course = await builder.CreateAndReturnCourse();
            var feeTemplate = await builder.CreateAndReturnFeeTemplate(course!.CourseId);
            var enrollment = await builder.CreateAndReturnEnrollment(student!.StudentId, course!.CourseId);
            var feeAssessment = await builder.CreateAndReturnFeeAssessment(enrollment!.EnrollmentId, feeTemplate!.FeeTemplateId);
            var invoice = await builder.CreateAndReturnInvoice(student!.StudentId, feeAssessment!.FeeAssessmentId, course!.CourseId);


            var update = new UpdateInvoiceDto
            {
                InvoiceId = invoice!.InvoiceId,
                StudentId = invoice.StudentId,
                CourseId = invoice.CourseId,
                FeeAssessmentId = invoice.FeeAssessmentId,
                InvoiceStatus = InvoiceStatus.Issued,
                IsActive = true,
                PaidDate = DateTimeOffset.UtcNow

            };
            //Act 
            var response = await _client.PutAsJsonAsync($"/api/invoice/{invoice.InvoiceId}", update);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }

        [TestMethod]
        public async Task Delete_WhenInvoiceExists_Return200()
        {
            //Arrange
            var student = await builder.CreateAndReturnStudent();
            var course = await builder.CreateAndReturnCourse();
            var feeTemplate = await builder.CreateAndReturnFeeTemplate(course!.CourseId);
            var enrollment = await builder.CreateAndReturnEnrollment(student!.StudentId, course!.CourseId);
            var feeAssessment = await builder.CreateAndReturnFeeAssessment(enrollment!.EnrollmentId, feeTemplate!.FeeTemplateId);
            var invoice = await builder.CreateAndReturnInvoice(student!.StudentId, feeAssessment!.FeeAssessmentId, course!.CourseId);

            //Act 
            var response = await _client.DeleteAsync($"/api/invoice/{invoice!.InvoiceId}");

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }

        #endregion

        #region Negative Part 
        [TestMethod]
        public async Task Create_WhenValidationFailed_Return400()
        {
            //Arrange
            var create = new CreateInvoiceDto
            {
                CourseId = 11, // pass invalid courseId 
            };
            //Act 
            var response = await _client.PostAsJsonAsync("/api/invoice", create);
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [TestMethod]
        public async Task GetById_When_Return404()
        {
            //Act 
            var response = await _client.GetAsync($"/api/invoice/{212322}");

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenInvoiceDoesNotExists_Return400()
        {
            //Arrange 
            var update = new UpdateInvoiceDto
            {
                InvoiceId = 12288,
                InvoiceStatus = InvoiceStatus.Cancelled,
                IsActive = true
            };

            //Act 
            var response = await _client.PutAsJsonAsync($"/api/invoice/{update.InvoiceId}", update);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [TestMethod]
        public async Task Update_WhenRouteAndBodyId_Return400()
        {
            //Arrange

            //Arrange
            var student = await builder.CreateAndReturnStudent();
            var course = await builder.CreateAndReturnCourse();
            var feeTemplate = await builder.CreateAndReturnFeeTemplate(course!.CourseId);
            var enrollment = await builder.CreateAndReturnEnrollment(student!.StudentId, course!.CourseId);
            var feeAssessment = await builder.CreateAndReturnFeeAssessment(enrollment!.EnrollmentId, feeTemplate!.FeeTemplateId);
            var invoice = await builder.CreateAndReturnInvoice(student!.StudentId, feeAssessment!.FeeAssessmentId, course!.CourseId);


            var update = new UpdateInvoiceDto
            {
                InvoiceId = 9999,
                FeeAssessmentId = invoice!.FeeAssessmentId,
                CourseId = invoice!.CourseId,
                StudentId = invoice!.StudentId,
                InvoiceStatus = InvoiceStatus.Issued,
                IsActive = true,
                PaidDate = DateTimeOffset.UtcNow

            };
            //Act 
            var response = await _client.PutAsJsonAsync($"/api/invoice/{invoice!.InvoiceId}", update);
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [TestMethod]
        public async Task Delete_When_Return400()
        {
            //Act 
            var response = await _client.DeleteAsync($"/api/invoice/{22224}");

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }

        #endregion
    }
}
