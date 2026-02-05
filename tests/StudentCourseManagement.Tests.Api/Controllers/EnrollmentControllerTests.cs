using StudentCourseManagement.Application.DTOs.Enrollments;
using StudentCourseManagement.Tests.Api.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace StudentCourseManagement.Tests.Api.Controllers
{
    [TestClass]
    public class EnrollmentControllerTests
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
        public async Task GetAllEnrollments_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Enrollment");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetActiveEnrollments_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Enrollment/active");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetInactiveEnrollments_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Enrollment/inactive");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetEnrollmentById_ValidId_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Enrollment/1");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetEnrollmentById_InvalidId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/Enrollment/99999");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task CreateEnrollment_ValidData_ReturnsCreated()
        {
            var enrollment = new CreateEnrollmentDto
            {
                StudentId = 1,
                CourseId = 1
            };

            var response = await _client.PostAsJsonAsync("/api/Enrollment", enrollment);
            Assert.IsTrue(
                response.StatusCode == HttpStatusCode.Created ||
                response.StatusCode == HttpStatusCode.OK
            );
        }

        [TestMethod]
        public async Task CreateEnrollment_InvalidStudentId_ReturnsBadRequest()
        {
            var enrollment = new CreateEnrollmentDto
            {
                StudentId = 99999,
                CourseId = 1
            };

            var response = await _client.PostAsJsonAsync("/api/Enrollment", enrollment);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task CreateEnrollment_InvalidCourseId_ReturnsBadRequest()
        {
            var enrollment = new CreateEnrollmentDto
            {
                StudentId = 1,
                CourseId = 99999
            };

            var response = await _client.PostAsJsonAsync("/api/Enrollment", enrollment);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task UpdateEnrollment_InvalidId_ReturnsNotFound()
        {
            var enrollment = new UpdateEnrollmentDto
            {
                StudentId = 1,
                CourseId = 1
            };

            var response = await _client.PutAsJsonAsync("/api/Enrollment/99999", enrollment);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task DeleteEnrollment_InvalidId_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("/api/Enrollment/99999");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task GetEnrollmentStatistics_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Enrollment/Enrollment-Statistics");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
