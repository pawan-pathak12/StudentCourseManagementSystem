using StudentCourseManagement.Application.DTOs.FInancialModule.Payments;
using StudentCourseManagement.Tests.Api.Builders;
using System.Net;
using System.Net.Http.Json;

namespace StudentCourseManagement.Tests.Api.Controllers.FinancialModule
{
    [TestClass]
    public class PaymentControllerTests : IntegrationTestBase
    {
        #region Happy Path

        // happy path means: user sends correct data => system works => success response

        [TestMethod]
        public async Task Create_WhenRequestIsValid_Return201()
        {
            // Arrange
            var testData = await SetupPaymentTestData();

            decimal amount = 100.22m;  // amount that is paid 
            var payment = new CreatePaymentDto
            {
                InvoiceId = testData!.InvoiceId,
                PaymentDate = DateTimeOffset.UtcNow,
                PaymentMethodId = testData.PaymentMethodId,
                StudentId = testData.StudentId,
                Amount = amount
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/payment", payment);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_WhenPaymentMethodExists_Return200()
        {
            // Arrange
            var testData = await SetupPaymentTestData();
            var payment = await builder.CreateAndReturnPayment(testData!.InvoiceId, testData!.PaymentMethodId, 100);
            // Act
            var response = await _client.GetAsync($"/api/payment/{payment!.PaymentId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetAll_WhenPaymentMethodsExist_ReturnOk()
        {
            // Arrange
            var testData = await SetupPaymentTestData();
            await builder.CreateAndReturnPayment(testData!.InvoiceId, testData!.PaymentMethodId, 100);

            // Act
            var response = await _client.GetAsync("/api/payment");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenRequestIsValid_Return200()
        {
            // Arrange
            var testData = await SetupPaymentTestData();
            var payment = await builder.CreateAndReturnPayment(testData!.InvoiceId, testData!.PaymentMethodId, 100);
            var updatePayment = new UpdatePaymentDto
            {
                PaymentId = payment!.PaymentId,
                IsActive = true,
                StudentId = testData!.StudentId,
                InvoiceId = testData!.InvoiceId,
                Amount = 150,
                PaymentMethodId = payment!.PaymentMethodId
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/payment/{payment!.PaymentId}", updatePayment);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_WhenPaymentMethodExists_Return204()
        {
            // Arrange
            var testData = await SetupPaymentTestData();
            var payment = await builder.CreateAndReturnPayment(testData!.InvoiceId, testData!.PaymentMethodId, 100);
            // Act
            var response = await _client.DeleteAsync($"/api/payment/{payment!.PaymentId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Unhappy Path

        // unhappy path means: user sends incorrect data => system rejects => error response

        [TestMethod]
        public async Task Create_WhenDataInvalid_Return400()
        {
            // Arrange
            var paymentMethod = await builder.CreateAndReturnPaymentMethod();

            var payment = new CreatePaymentDto  //missing invoiceId
            {
                Amount = 100.0m,
                PaymentMethodId = paymentMethod!.PaymentMethodId
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/payment", payment);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_WhenPaymentNotExists_Return404()
        {
            // Act
            var response = await _client.GetAsync("/api/payment/99999");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenPaymentNotExists_Return400()
        {
            // Arrange
            var paymentMethod = await builder.CreateAndReturnPaymentMethod();
            var updateDto = new UpdatePaymentDto
            {
                PaymentId = 111,
                PaymentMethodId = paymentMethod!.PaymentMethodId,
                IsActive = true
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/payment/{111}", updateDto);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_WhenPaymentNotExists_Return400()
        {
            // Act
            var response = await _client.DeleteAsync("/api/payment/99999");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region Helper Methhod 

        public async Task<PaymentTestData?> SetupPaymentTestData()
        {
            var student = await builder.CreateAndReturnStudent();
            if (student == null)
            {
                return null;
            }
            var course = await builder.CreateAndReturnCourse();
            if (course == null)
            {
                return null;
            }
            var enrollment = await builder.CreateAndReturnEnrollment(student.StudentId, course.CourseId);
            if (enrollment == null)
            {
                return null;
            }
            var feeTemplate = await builder.CreateAndReturnFeeTemplate(course.CourseId);
            if (feeTemplate == null)
            {
                return null;
            }
            var feeAssessment = await builder.CreateAndReturnFeeAssessment(enrollment!.EnrollmentId, feeTemplate.FeeTemplateId);
            if (feeAssessment == null)
            {
                return null;
            }
            var invoice = await builder.CreateAndReturnInvoice(student.StudentId, feeAssessment.FeeAssessmentId, course!.CourseId);
            if (invoice == null)
            {
                return null;
            }
            var paymentMethod = await builder.CreateAndReturnPaymentMethod();
            if (paymentMethod == null)
            {
                return null;
            }

            return new PaymentTestData
            {
                StudentId = student.StudentId,
                CourseId = course.CourseId,
                EnrollmentId = enrollment.EnrollmentId,
                FeeAssessmentId = feeAssessment.FeeAssessmentId,
                InvoiceId = invoice.InvoiceId,
                PaymentMethodId = paymentMethod.PaymentMethodId,
                InvoiceAmount = invoice.TotalAmount
            };

        }
        #endregion
    }
}
