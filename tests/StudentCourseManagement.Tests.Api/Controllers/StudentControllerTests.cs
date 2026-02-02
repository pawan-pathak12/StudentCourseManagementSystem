using StudentCourseManagement.Application.DTOs.Students;
using StudentCourseManagement.Tests.Api.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace StudentCourseManagement.Tests.Api.Controllers
{
    [TestClass]
    // since jwt token is used so this all test is failed as all the endpoint is authorize
    public class StudentControllerTests
    {
        private static CustomWebApplicationFactory _factory = null!;
        private static HttpClient _client = null!;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [TestMethod]
        public async Task GetAllStudents_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Student");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetStudentById_ValidId_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Student/1");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetStudentById_InvalidId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/Student/99999");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task CreateStudent_ValidData_ReturnsCreated()
        {
            var student = new CreateStudentDto
            {
                Name = "Test",
                Address = "ktm",
                Email = $"test{Guid.NewGuid()}@example.com",
                DOB = new DateTime(2000, 1, 1)
            };

            var response = await _client.PostAsJsonAsync("/api/Student", student);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public async Task CreateStudent_InvalidData_ReturnsBadRequest()
        {
            var student = new CreateStudentDto
            {
                Name = "",
                Address = "",
                Email = "invalid"
            };

            var response = await _client.PostAsJsonAsync("/api/Student", student);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task UpdateStudent_InvalidId_ReturnsNotFound()
        {
            var student = new UpdateStudentDto
            {
                Name = "Test",
                Address = "ktm",
                Email = "test@example.com"
            };

            var response = await _client.PutAsJsonAsync("/api/Student/99999", student);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteStudent_InvalidId_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("/api/Student/99999");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
