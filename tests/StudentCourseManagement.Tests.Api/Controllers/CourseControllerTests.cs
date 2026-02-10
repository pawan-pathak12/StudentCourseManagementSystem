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
        }

        [TestInitialize]
        public void TestInit()
        {
            _client = _factory.CreateClient();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        #region New - Happy Part 
        [TestMethod]
        public async Task Create_WhenDataIsValid_Return201()
        {
            //Arrange
            var course = new CreateCourseDto
            {
                Title = "Testing",
                Capacity = 100,
                Code = "CS101",
                Credits = 10
            };

            //Act
            var createResponse = await _client.PostAsJsonAsync("/api/course", course);

            //Assert 
            Assert.AreEqual(HttpStatusCode.Created, createResponse.StatusCode);
        }

        [TestMethod]
        public async Task GetAll_WhenStudentExists_Return200()
        {
            //Arrange
            var course = new CreateCourseDto
            {
                Title = "Testing",
                Capacity = 100,
                Code = "CS101",
                Credits = 10
            };
            await _client.PostAsJsonAsync("/api/course", course);

            //Act
            var courses = await _client.GetAsync("/api/student");

            //Assert 
            Assert.AreEqual(HttpStatusCode.OK, courses.StatusCode);
        }

        public async Task GetById_WhenStudentExists_Return200()
        {
            //Arrange

            var course = new CreateCourseDto
            {
                Title = "Testing",
                Capacity = 100,
                Code = "CS101",
                Credits = 10
            };
            var createResponse = await _client.PostAsJsonAsync("/api/course", course);
            var createdCourse = await createResponse.Content
                .ReadFromJsonAsync<CourseResponseDto>();

            //Act
            var response = await _client.GetAsync($"/api/course/{createdCourse!.CourseId}");

            //Assert 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenStudentExists_Return204()
        {
            //Arrange
            var course = new CreateCourseDto
            {
                Title = "Testing",
                Capacity = 100,
                Code = "CS101",
                Credits = 10
            };
            var createdResposne = await _client.PostAsJsonAsync("/api/course", course);
            var createdCourse = await createdResposne.Content.
                ReadFromJsonAsync<CourseResponseDto>();

            var updateCourse = new UpdateCourseDto
            {
                CourseId = createdCourse!.CourseId,
                Description = "New DEscription",
                IsActive = true
            };

            //Act
            var resposne = await _client.PutAsJsonAsync($"/api/course/{createdCourse!.CourseId}", updateCourse);

            //Assert 
            Assert.AreEqual(HttpStatusCode.NoContent, resposne.StatusCode);
        }

        [TestMethod]
        public async Task Delete_WhneStudentExists_Return204()
        {
            //Arrange
            var course = new CreateCourseDto
            {
                Title = "Testing",
                Capacity = 100,
                Code = "CS101",
                Credits = 10
            };

            var createdResposne = await _client.PostAsJsonAsync("/api/course", course);
            var createdCourse = await createdResposne.Content.
                ReadFromJsonAsync<CourseResponseDto>();

            //Act
            var resposne = await _client.DeleteAsync($"/api/course/{createdCourse!.CourseId}");

            //Assert 
            Assert.AreEqual(HttpStatusCode.NoContent, resposne.StatusCode);
        }
        #endregion

        #region New - Negative Part 

        [TestMethod]
        public async Task Create_WhenRequestIsInvalid_Return400()
        {
            //Arrange
            var course = new CreateCourseDto
            {
                Title = "",
                Capacity = -11
            };

            //Act
            var response = await _client.PostAsJsonAsync("/api/course", course);

            //Assert 
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [TestMethod]
        public async Task GetById_WhenCourseDoesNotExists_Return404()
        {

            //Act
            var response = await _client.GetAsync($"/api/course/{4445550}");

            //Assert 
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }



        [TestMethod]
        public async Task Update_WhenCoursetDoesNotExists_Return404()
        {
            //Arrange
            var request = new UpdateCourseDto
            {
                Title = "Hello WOrld",
                Capacity = 44
            };

            //Act
            var response = await _client.PostAsJsonAsync($"/api/course/{9999}", request);

            //Assert 
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }


        [TestMethod]
        public async Task Update_WhenRouteIdAndBodyIdMisMatch_Return400()
        {
            //Arrange
            var course = new CreateCourseDto
            {
                Title = "Testing",
                Capacity = 100,
                Code = "CS101",
                Credits = 10
            };
            var createResponse = await _client.PostAsJsonAsync("/api/course", course);
            var createdCourse = await createResponse.Content
                .ReadFromJsonAsync<CourseResponseDto>();

            var request = new UpdateCourseDto
            {
                CourseId = createdCourse!.CourseId,
                Title = "c# basic ",
                Capacity = 111
            };

            //Act
            var response = await _client.PutAsJsonAsync($"/api.course/{111}", request);
            //Assert 
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        }


        [TestMethod]
        public async Task Delete_WhenCoursetDoesNotExist_Return404()
        {

            //Act
            var resposne = await _client.DeleteAsync($"/api/course{13242424}");

            //Assert 
            Assert.AreEqual(HttpStatusCode.NotFound, resposne.StatusCode);
        }
        #endregion



    }
}