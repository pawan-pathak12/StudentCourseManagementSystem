using StudentCourseManagement.Application.DTOs.FInancialModule.InvoiceLineItems;
using StudentCourseManagement.Application.DTOs.FInancialModule.Invoices;
using System.Net;
using System.Net.Http.Json;

namespace StudentCourseManagement.Tests.Api.Controllers.FinancialModule
{
    [TestClass]
    public class InviceLineItemsTests : IntegrationTestBase
    {
        //need : id : invoice , feetemplate , courseId

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
            var invoice = await builder.CreateAndReturnInvoice(student!.StudentId, course!.CourseId, feeAssessment!.FeeAssessmentId);


            var lineItem = new CreateInvoiceDto
            {
                CourseId = course!.CourseId,
                InvoiceId = invoice!.InvoiceId,
                feeTemplateId = feeTemplate!.FeeTemplateId
            };

            //Act 
            var response = await _client.PostAsJsonAsync("/api/invoiceLineItem", lineItem);

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
            var invoice = await builder.CreateAndReturnInvoice(student!.StudentId, course!.CourseId, feeAssessment!.FeeAssessmentId);
            await builder.CreateInvoiceLineItem(invoice!.InvoiceId);

            //Act 
            var response = await _client.GetAsync("/api/invoiceLineItem");
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
        [TestMethod]
        public async Task GetById_WhenInvoiceExistis_Return201()
        {
            //Arrange
            var student = await builder.CreateAndReturnStudent();
            var course = await builder.CreateAndReturnCourse();
            var feeTemplate = await builder.CreateAndReturnFeeTemplate(course!.CourseId);
            var enrollment = await builder.CreateAndReturnEnrollment(student!.StudentId, course!.CourseId);
            var feeAssessment = await builder.CreateAndReturnFeeAssessment(enrollment!.EnrollmentId, feeTemplate!.FeeTemplateId);
            var invoice = await builder.CreateAndReturnInvoice(student!.StudentId, course!.CourseId, feeAssessment!.FeeAssessmentId);
            var lineItem = await builder.CreateInvoiceLineItem(invoice!.InvoiceId);
            //Act 
            var response = await _client.GetAsync($"/api/invoiceLineItem/{lineItem!.InvoiceLineItemId}");

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
            var invoice = await builder.CreateAndReturnInvoice(student!.StudentId, course!.CourseId, feeAssessment!.FeeAssessmentId);
            var lineItem = await builder.CreateInvoiceLineItem(invoice!.InvoiceId);


            var update = new UpdateInvoiceLineItemDto
            {
                LineItemId = lineItem!.InvoiceLineItemId,
                IsActive = true
            };
            //Act 
            var response = await _client.PutAsJsonAsync($"/api/invoiceLineItem/{lineItem!.InvoiceLineItemId}", update);
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
            var invoice = await builder.CreateAndReturnInvoice(student!.StudentId, course!.CourseId, feeAssessment!.FeeAssessmentId);
            var lineItem = await builder.CreateInvoiceLineItem(invoice!.InvoiceId);

            //Act 
            var response = await _client.DeleteAsync($"/api/invoiceLineItem/{lineItem!.InvoiceLineItemId}");

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }

        #endregion

        #region Negative Part 
        public async Task Create_WhenValidationFailed_Return400()
        {
            //Arrange
            var course = await builder.CreateAndReturnCourse();
            var feeTemplate = await builder.CreateAndReturnFeeTemplate(course!.CourseId);

            var create = new CreateInvoiceLineItemDto  //misisng invoice Id 
            {
                CourseId = course!.CourseId,
                FeeTemplateId = feeTemplate!.FeeTemplateId,
                Amount = 100
            };
            //Act 
            var response = await _client.PostAsJsonAsync("/api/invoiceLineItem", create);
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [TestMethod]
        public async Task GetById_When_Return404()
        {
            //Act 
            var response = await _client.GetAsync($"/api/invoiceLineItem/{212322}");

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenInvoiceLineItemDoesNotExists_Return404()
        {
            //Arrange 
            var update = new UpdateInvoiceLineItemDto
            {
                LineItemId = 9999,
                Amount = 1111
            };

            //Act 
            var response = await _client.PutAsJsonAsync($"/api/invoiceLineItem/{update.LineItemId}", update);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

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
            var invoice = await builder.CreateAndReturnInvoice(student!.StudentId, course!.CourseId, feeAssessment!.FeeAssessmentId);
            var LineItem = await builder.CreateInvoiceLineItem(invoice!.InvoiceId);

            var update = new UpdateInvoiceLineItemDto
            {
                LineItemId = 222111, // wrrong id is passed 
                Amount = 111,
                IsActive = true

            };
            //Act 
            var response = await _client.PutAsJsonAsync($"/api/invoiceLineItem/{LineItem!.InvoiceLineItemId}", update);
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [TestMethod]
        public async Task Delete_When_Return404()
        {
            //Act 
            var response = await _client.DeleteAsync($"/api/invoiceLineItem/{222214}");

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

        }

        #endregion
    }
}
