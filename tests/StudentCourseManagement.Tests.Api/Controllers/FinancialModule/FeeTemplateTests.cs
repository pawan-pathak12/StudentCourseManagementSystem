using StudentCourseManagement.Application.DTOs.FInancialModule.feeTemplates;
using System.Net;
using System.Net.Http.Json;

namespace StudentCourseManagement.Tests.Api.Controllers.FinancialModule
{
    [TestClass]
    public class FeeTemplateTests : IntegrationTestBase
    {
        #region Happy Part 

        [TestMethod]
        public async Task Create_WhenValid_Return201()
        {
            //Arrange 
            var course = await builder.CreateCourse();
            var feeTemplate = new CreateFeeTemplateDto
            {
                CourseId = course!.CourseId,
                Amount = 1122,
                Description = "Testing"
            };

            //Act 

            var response = await _client.PostAsJsonAsync("/api/feeTemplate", feeTemplate);

            //Assert 
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
        [TestMethod]
        public async Task GetAll_WhenTemplateExists_Return200()
        {
            //Arrange 
            var course = await builder.CreateCourse();
            var feeTemplate = await builder.CreateFeeTemplate(course.CourseId);

            //Act 
            var response = await _client.GetAsync("/api/feeTemplate");
            //Assert 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_WhenExistingTemplateExists_Return200()
        {
            //Arrange 
            var course = await builder.CreateCourse();
            var feeTemplate = await builder.CreateFeeTemplate(course.CourseId);

            //Act 
            var response = await _client.GetAsync($"/api/feeTemplate/{feeTemplate!.FeeTemplateId}");
            //Assert 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenExistinhTemplateExists_Return200()
        {
            //Arrange 
            var course = await builder.CreateCourse();
            var feeTemplate = await builder.CreateFeeTemplate(course.CourseId);

            var updateFeeTemplate = new UpdateFeeTemplateDto
            {
                FeeTemplateId = feeTemplate!.FeeTemplateId,
                Amount = 1000,
                Description = "hello ",
                IsActive = true
            };
            //Act 
            var response = await _client.PutAsJsonAsync($"/api/feeTemplate/{feeTemplate!.FeeTemplateId}", updateFeeTemplate);

            //Assert 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_WhenFeeTemplateExists_Return200()
        {
            //Arrange 
            var course = await builder.CreateCourse();
            var feeTemplate = await builder.CreateFeeTemplate(course.CourseId);

            //Act 
            var response = await _client.DeleteAsync($"/api/feeTemplate/{feeTemplate!.FeeTemplateId}");
            //Assert 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Negative Part
        [TestMethod]
        public async Task Create_WhenValidationFailed_Return400()
        {
            //Arrange 
            var feeTemplate = new CreateFeeTemplateDto
            {
                Amount = 1000,
                Description = "ewin g"
            };
            //Act 
            var response = await _client.PostAsJsonAsync("/api/feeTemplate", feeTemplate);

            //Assert 
            Assert.AreEqual(HttpStatusCode.BadGateway, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_WhenFeeTemplateNotFound_Return404()
        {
            //Act 
            var response = await _client.GetAsync($"/api/feeTemplate/{11221312}");

            //Assert 
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenFeeTemplateNotFound_Return404()
        {
            //Arrange 
            var updateData = new UpdateFeeTemplateDto
            {
                FeeTemplateId = 1235677,
                Amount = 1111
            };

            //Act
            var response = await _client.PutAsJsonAsync($"/api/feeTemplate/{updateData!.FeeTemplateId}", updateData);

            //Assert 
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenRouteAndBodyIdMissMatched_Return400()
        {
            //Arrange 
            var course = await builder.CreateCourse();
            var feeTemplate = await builder.CreateFeeTemplate(course.CourseId);

            var updateData = new UpdateFeeTemplateDto
            {
                FeeTemplateId = 1235677, // pass wrong id 
                Amount = 1111
            };

            //Act 
            var response = await _client.PutAsJsonAsync($"/api/feeTemplate/{feeTemplate!.FeeTemplateId}", updateData);
            //Assert 
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [TestMethod]
        public async Task Delete_WhentemplateIdNotFound_Return404()
        {
            //Act 
            var response = await _client.DeleteAsync($"/api/feeTemplate/1111");

            //Assert 
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        #endregion
    }
}
