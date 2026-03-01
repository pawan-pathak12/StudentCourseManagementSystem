using StudentCourseManagement.Application.DTOs.Courses;
using System.Net;
using System.Net.Http.Json;

namespace StudentCourseManagement.Tests.Api.Controllers
{
    [TestClass]
    public class CourseControllerTests : IntegrationTestBase
    {

        #region New - Happy Part 
        [TestMethod]
        public async Task Create_WhenDataIsValid_Return201()
        {
            var rand = new Random();
            //Arrange
            var course = new CreateCourseDto
            {
                Code = $"C{rand.Next(1000, 9999)}",
                Title = "Introduction to Computer Science and Technology",
                Credits = 3,
                Description = "Foundational course covering programming basics, algorithms, and problem-solving.",
                Instructor = "Jon Doe",

                StartDate = DateTimeOffset.UtcNow.AddDays(7),
                EndDate = DateTimeOffset.UtcNow.AddDays(120),

                Capacity = 50,
                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(1),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(30)
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
            await builder.CreateAndReturnCourse();

            //Act
            var courses = await _client.GetAsync("/api/course");

            //Assert 
            Assert.AreEqual(HttpStatusCode.OK, courses.StatusCode);
        }

        [TestMethod]
        public async Task GetById_WhenStudentExists_Return200()
        {
            //Arrange
            var course = await builder.CreateAndReturnCourse();
            //Act
            var response = await _client.GetAsync($"/api/course/{course!.CourseId}");

            //Assert 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenStudentExists_Return204()
        {
            //Arrange
            var course = await builder.CreateAndReturnCourse();

            var updateCourse = new UpdateCourseDto
            {
                CourseId = course!.CourseId,
                Description = "New DEscription",
                IsActive = true,
                Code = course.Code,
                Title = course.Title,
                Credits = course.Credits,
                Capacity = course.Capacity,
                StartDate = course.StartDate,
                Instructor = course.Instructor
            };

            //Act
            var resposne = await _client.PutAsJsonAsync($"/api/course/{course!.CourseId}", updateCourse);

            //Assert 
            Assert.AreEqual(HttpStatusCode.NoContent, resposne.StatusCode);
        }

        [TestMethod]
        public async Task Delete_WhneStudentExists_Return204()
        {
            //Arrange
            var course = await builder.CreateAndReturnCourse();

            //Act
            var resposne = await _client.DeleteAsync($"/api/course/{course!.CourseId}");

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
                Capacity = 11
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
            var response = await _client.PutAsJsonAsync($"/api/course/{9999}", request);

            //Assert 
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [TestMethod]
        public async Task Update_WhenRouteIdAndBodyIdMisMatch_Return404()
        {
            //Arrange
            var course = await builder.CreateAndReturnCourse();

            var request = new UpdateCourseDto
            {
                CourseId = 9999,
                Title = "c# basic ",
                Capacity = 111
            };

            //Act
            var response = await _client.PutAsJsonAsync($"/api.course/{course!.CourseId}", request);
            //Assert 
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);

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