using StudentCourseManagement.Application.DTOs.Students;
using System.Net;
using System.Net.Http.Json;

namespace StudentCourseManagement.Tests.Api.Controllers
{
    [TestClass]

    public class StudentControllerTests : IntegrationTestBase
    {

        #region happy path 

        // happy path means : use sends correct data => system works => success resposne 

        [TestMethod]
        public async Task Create_WhenRequestIsValid_Return200()
        {
            //Arrange 
            var student = new CreateStudentDto
            {
                Name = "Pawan",
                Address = "Haldibari",
                Email = $"tester{RandomNumberGenerator()}@gmail.com",
                DOB = DateTimeOffset.UtcNow.AddYears(-20),
                Gender = "Male"
            };

            //Act 
            var response = await _client.PostAsJsonAsync("/api/student", student);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_WhenStudentExists_Return200()
        {
            //Arrange 
            var studentData = await builder.CreateAndReturnStudent();
            //Act 

            var response = await _client.GetAsync($"/api/student/{studentData!.StudentId}");
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetAll_WhenStudentExists_ReturnOk()
        {
            //Arrange 
            var studentData = await builder.CreateAndReturnStudent();

            //Act 
            var resposne = await _client.GetAsync("/api/student");

            //Assert 
            Assert.AreEqual(HttpStatusCode.OK, resposne.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenRequestIsValid_Return200()
        {
            //Arrange 
            var studentData = await builder.CreateAndReturnStudent();
            var student2 = new UpdateStudentDto
            {
                Id = (int)studentData!.StudentId,
                Email = studentData.Email,
                DOB = studentData.DOB,
                Name = "Ram Nath",
                Address = "Ktm"
            };

            //Act 

            var updateResponse = await _client.PutAsJsonAsync($"/api/student/{(int)studentData!.StudentId}", student2);

            //Assert 

            Assert.AreEqual(HttpStatusCode.OK, updateResponse.StatusCode);
        }

        [TestMethod]
        public async Task Delete_WhenStudentExists_Return204()
        {
            //Arrange 
            var studentData = await builder.CreateAndReturnStudent();

            //Act 
            var deleteResposne = await _client.DeleteAsync($"/api/student/{studentData!.StudentId}");

            //Assert 
            Assert.AreEqual(HttpStatusCode.OK, deleteResposne.StatusCode);

        }

        #endregion

        #region Negative Path 
        /*negative path means :
         use sends wrong / unexpected / missing / invalid data => system must fall correctly
        */

        [TestMethod]
        public async Task GetById_WhenStudentDoesNotExist_Return404()
        {
            //Act 
            var response = await _client.GetAsync($"/api/student/{9999999}");

            //Arrange
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenStudentDoesNotExist_Return404()
        {
            //Arrange 
            int id = 999988;
            var request = new UpdateStudentDto
            {
                Name = "Updted Name ",
                Address = "Updaed Address"
            };

            //Act 
            var response = await _client.PutAsJsonAsync($"/api/student/{id}", request);

            //Assert 
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenRouteIdAndBodyIdMissMatch_Return400()
        {
            //Arrmage 
            var request = new UpdateStudentDto
            {
                Id = 99,
                Name = "String"
            };

            //Act 
            var response = await _client.PutAsJsonAsync($"/api/student/{1}", request);

            //Assert 
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
        #endregion


    }
}
