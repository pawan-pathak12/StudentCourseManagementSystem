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
            var rand = new Random();
            var course = await builder.CreateAndReturnCourse();
            var feeTemplate = new CreateFeeTemplateDto
            {
                Name = $"Lab Fee {rand.Next(1000, 9999)}",
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
            var course = await builder.CreateAndReturnCourse();
            var feeTemplate = await builder.CreateAndReturnFeeTemplate(course.CourseId);

            //Act 
            var response = await _client.GetAsync("/api/feeTemplate");
            //Assert 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_WhenExistingTemplateExists_Return200()
        {
            //Arrange 
            var course = await builder.CreateAndReturnCourse();
            var feeTemplate = await builder.CreateAndReturnFeeTemplate(course.CourseId);

            //Act 
            var response = await _client.GetAsync($"/api/feeTemplate/{feeTemplate!.FeeTemplateId}");
            //Assert 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenExistingTemplateExists_Return200()
        {
            //Arrange 
            var course = await builder.CreateAndReturnCourse();
            Assert.IsNotNull(course);
            var feeTemplate = await builder.CreateAndReturnFeeTemplate(course.CourseId);
            Assert.IsNotNull(feeTemplate);
            var updateFeeTemplate = new UpdateFeeTemplateDto
            {
                Name = feeTemplate?.Name,
                FeeTemplateId = feeTemplate!.FeeTemplateId,
                CourseId = feeTemplate.CourseId,
                Amount = 1000,
                Description = "Testing update endpoint ",
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
            var course = await builder.CreateAndReturnCourse();
            var feeTemplate = await builder.CreateAndReturnFeeTemplate(course.CourseId);

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
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
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
        public async Task Update_WhenFeeTemplateNotFound_Return400()
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
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenRouteAndBodyIdMissMatched_Return400()
        {
            //Arrange 
            var course = await builder.CreateAndReturnCourse();
            var feeTemplate = await builder.CreateAndReturnFeeTemplate(course.CourseId);

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
        public async Task Delete_WhentemplateIdNotFound_Return400()
        {
            //Act 
            var response = await _client.DeleteAsync($"/api/feeTemplate/1111");

            //Assert 
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
        #endregion
    }
}
