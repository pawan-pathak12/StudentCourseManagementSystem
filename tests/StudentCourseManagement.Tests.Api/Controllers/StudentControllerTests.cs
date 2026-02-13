using StudentCourseManagement.Application.DTOs.Students;
using System.Net;
using System.Net.Http.Json;

namespace StudentCourseManagement.Tests.Api.Controllers
{
    [TestClass]

    // since jwt token is used so this all test is failed as all the endpoint is authorize
    // error to fix : as CreateUser method is not under testclass so data is not deleted once test is completed 
    // bug : in generate jwt token user role and id is nnot returned so left it fix 
    public class StudentControllerTests : IntegrationTestBase
    {

        #region happy path 

        // happy path means : use sends correct data => system works => success resposne 

        [TestMethod]
        public async Task Create_WhenRequestIsValid_Return201()
        {
            //Arrange 
            var student = new CreateStudentDto
            {
                Name = "Pawan",
                Address = "Haldibari"
            };

            //Act 
            var response = await _client.PostAsJsonAsync("/api/Student", student);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_WhenStudentExists_Return200()
        {
            // Arrange 

            var student = new CreateStudentDto
            {
                Name = "Pawan",
                Address = "Haldibari"
            };
            var createResponse = await _client.PostAsJsonAsync("/api/Student", student);

            var createdStudent = await createResponse.Content.
                ReadFromJsonAsync<StudentResponseDto>();

            //Act 

            var response = await _client.GetAsync($"/api/Student/{createdStudent!.StudentId}");
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetAll_WhenStudentExists_ReturnOk()
        {
            //Arrange 
            var student = new CreateStudentDto
            {
                Name = "Pawan",
                Address = "Haldibari"
            };

            var response = await _client.PostAsJsonAsync("/api/student", student);

            //Act 
            var resposne = await _client.GetAsync("/api/student/");

            //Assert 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }


        [TestMethod]
        public async Task Update_WhenRequestIsValid_Return200()
        {
            //Arrange 

            var student = new CreateStudentDto
            {
                Name = "Pawan",
                Address = "Haldibari"
            };

            var studentResponse = await _client.PostAsJsonAsync("/api/student", student);
            var createdStudent = await studentResponse.Content.
                ReadFromJsonAsync<StudentResponseDto>();


            var student2 = new CreateStudentDto
            {
                Name = "Ram Nath",
                Address = "Ktm"
            };

            //Act 
            var updateResponse = await _client.PutAsJsonAsync($"/api/student/{createdStudent!.StudentId}", student2);
            //Assert 
            Assert.AreEqual(HttpStatusCode.OK, updateResponse.StatusCode);

        }
        [TestMethod]
        public async Task Delete_WhenStudentExists_Return204()
        {
            //Arrange 
            var studentData = await builder.CreateStudent();

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
            var response = await _client.GetAsync("/api/student/9999999");

            //Arrange
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenStudentDoesNotExist_Return404()
        {
            //Arrange 
            var request = new UpdateStudentDto
            {
                Name = "Updted Name ",
                Address = "Updaed Address"
            };

            //Act 
            var response = await _client.PatchAsJsonAsync("/api/student/99999", request);

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
            var response = await _client.PutAsJsonAsync("/api/student/1", request);

            //Assert 
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
        #endregion
    }
}
