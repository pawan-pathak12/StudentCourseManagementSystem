using StudentCourseManagement.Application.DTOs.FInancialModule.PaymentMethods;
using System.Net;
using System.Net.Http.Json;

namespace StudentCourseManagement.Tests.Api.Controllers.FinancialModule;

[TestClass]
public class PaymentMethodControllerTests : IntegrationTestBase
{
    #region Happy Path

    // happy path means: user sends correct data => system works => success response

    [TestMethod]
    public async Task Create_WhenRequestIsValid_Return201()
    {
        // Arrange
        var paymentMethod = new CreatePaymentMethodDto
        {
            Name = $"Cash Payment {RandomNumberGenerator()}",
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/paymentmethod", paymentMethod);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
    }

    [TestMethod]
    public async Task GetById_WhenPaymentMethodExists_Return200()
    {
        // Arrange
        var paymentMethodData = await builder.CreatePaymentMethod();

        // Act
        var response = await _client.GetAsync($"/api/paymentmethod/{paymentMethodData!.PaymentMethodId}");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task GetAll_WhenPaymentMethodsExist_ReturnOk()
    {
        // Arrange
        var paymentMethodData = await builder.CreatePaymentMethod();

        // Act
        var response = await _client.GetAsync("/api/paymentmethod");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task Update_WhenRequestIsValid_Return200()
    {
        // Arrange
        var paymentMethodData = await builder.CreatePaymentMethod();
        var updateDto = new UpdatePaymentMethodDto
        {
            PaymentMethodId = paymentMethodData!.PaymentMethodId,
            Name = $"Updated Payment Method {RandomNumberGenerator()}",
            IsActive = true
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/paymentmethod/{paymentMethodData.PaymentMethodId}", updateDto);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task Delete_WhenPaymentMethodExists_Return204()
    {
        // Arrange
        var paymentMethodData = await builder.CreatePaymentMethod();

        // Act
        var response = await _client.DeleteAsync($"/api/paymentmethod/{paymentMethodData!.PaymentMethodId}");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region Unhappy Path

    // unhappy path means: user sends incorrect data => system rejects => error response

    [TestMethod]
    public async Task GetById_WhenPaymentMethodNotExists_Return404()
    {
        // Act
        var response = await _client.GetAsync("/api/paymentmethod/99999");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task Create_WhenDataInvalid_Return400()
    {
        // Arrange
        var paymentMethod = new CreatePaymentMethodDto
        {
            Name = "",  // Invalid: empty name
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/paymentmethod", paymentMethod);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task Update_WhenPaymentMethodNotExists_Return404()
    {
        // Arrange
        var updateDto = new UpdatePaymentMethodDto
        {
            PaymentMethodId = 99999,
            Name = "Non Existent",
            IsActive = true
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/paymentmethod/99999", updateDto);

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task Delete_WhenPaymentMethodNotExists_Return404()
    {
        // Act
        var response = await _client.DeleteAsync("/api/paymentmethod/99999");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion
}