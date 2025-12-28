using Microsoft.AspNetCore.Mvc.Testing;
using StudentCourseManagement.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

namespace StudentCourseManagement.Tests.Unit.Controllers.Students
{
    [TestClass]
    public class StudentControllerTests_Post
    {
        private HttpClient _client;

        [TestInitialize]
        public void Setup()
        {
            var factory = new WebApplicationFactory<Program>();
            _client = factory.CreateClient();
        }

        [TestMethod]
        public async Task CreateMember_WithValidData_ReturnOk()
        {
            //Arrange 
            var student = new Student
            {
                Name = "Pawan",
                Address = "Haldibari",
                Email = "pawan@gmail.com"
            };

            var response = await _client.PostAsJsonAsync("/api/student", student);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
    }
}
