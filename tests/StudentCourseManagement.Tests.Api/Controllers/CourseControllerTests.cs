using StudentCourseManagement.Application.DTOs.Courses;
using StudentCourseManagement.Tests.Api.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace StudentCourseManagement.Tests.Api.Controllers
{
    [TestClass]
    public class CourseControllerTests
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
        public async Task GetAllCourses_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Course");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetCourseById_ValidId_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Course/1");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetCourseById_InvalidId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/Course/99999");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task CreateCourse_ValidData_ReturnsCreated()
        {
            var course = new CreateCourseDto
            {
                Title = $"Test Course {Guid.NewGuid()}",
                Credits = 3,
                Instructor = "Test Instructor",
                Capacity = 30,
                StartDate = DateTime.UtcNow.AddDays(30),
                EndDate = DateTime.UtcNow.AddDays(120)
            };

            var response = await _client.PostAsJsonAsync("/api/Course", course);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public async Task CreateCourse_InvalidData_ReturnsBadRequest()
        {
            var course = new CreateCourseDto
            {
                Title = "",
                Credits = -1,
                Instructor = "",
                Capacity = 0
            };

            var response = await _client.PostAsJsonAsync("/api/Course", course);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task UpdateCourse_InvalidId_ReturnsNotFound()
        {
            var course = new UpdateCourseDto
            {
                Title = "Updated Course",
                Credits = 3,
                Instructor = "Updated Instructor",
                Capacity = 25
            };

            var response = await _client.PutAsJsonAsync("/api/Course/99999", course);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteCourse_InvalidId_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("/api/Course/99999");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}