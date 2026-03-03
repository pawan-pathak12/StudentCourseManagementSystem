using StudentCourseManagement.Application.DTOs.Enrollments;
using System.Net;
using System.Net.Http.Json;

namespace StudentCourseManagement.Tests.Api.Controllers
{
    [TestClass]
    public class EnrollmentControllerTests : IntegrationTestBase
    {
        #region New Happy Path 

        [TestMethod]
        public async Task Create_WhenStudentAndCourseExists_Return201()
        {
            //Arrange 
            var student = await builder.CreateAndReturnStudent();
            var course = await builder.CreateAndReturnCourse();

            var enrollmentRequest = new CreateEnrollmentDto
            {
                StudentId = student!.StudentId,
                CourseId = course!.CourseId
            };

            //Act
            var resposne = await _client.PostAsJsonAsync("/api/enrollment", enrollmentRequest);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, resposne.StatusCode);
        }

        [TestMethod]
        public async Task GetById_WhenEnrollmentExists_Return200()
        {
            //Arrange 

            var student = await builder.CreateAndReturnStudent();
            var course = await builder.CreateAndReturnCourse();

            var enrollment = await builder.CreateAndReturnEnrollment(student.StudentId, course.CourseId);

            //Act
            var existingEnrollment = await _client.GetAsync($"/api/enrollment/{enrollment!.EnrollmentId}");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, existingEnrollment.StatusCode);

        }
        [TestMethod]
        public async Task GetAll_WhenEnrollmentExists_Return200()
        {
            //Arrange 

            var student = await builder.CreateAndReturnStudent();
            var course = await builder.CreateAndReturnCourse();

            var enrollment = await builder.CreateAndReturnEnrollment(student.StudentId, course.CourseId);

            //Act
            var existingEnrollment = await _client.GetAsync("/api/enrollment/");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, existingEnrollment.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenEnrollmentExists_Return200()
        {
            //Arrange 
            var student = await builder.CreateAndReturnStudent();
            var course = await builder.CreateAndReturnCourse();

            var enrollment = await builder.CreateAndReturnEnrollment(student.StudentId, course.CourseId);

            var updateRequest = new UpdateEnrollmentDto
            {
                EnrollmentId = enrollment!.EnrollmentId,
                CourseId = enrollment!.CourseId,
                StudentId = enrollment!.StudentId,
                CancellationReason = "Just kidding"
            };

            //Act
            var updatedResponse = await _client.PutAsJsonAsync($"/api/enrollment/{enrollment!.EnrollmentId}", updateRequest);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, updatedResponse.StatusCode);
        }
        [TestMethod]
        public async Task Delete_WhenEnrollmentExists_Return204()
        {
            //Arrange 
            var student = await builder.CreateAndReturnStudent();
            var course = await builder.CreateAndReturnCourse();

            var enrollment = await builder.CreateAndReturnEnrollment(student.StudentId, course.CourseId);

            //Act
            var existingEnrollment = await _client.DeleteAsync($"/api/enrollment/{enrollment!.EnrollmentId}");

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, existingEnrollment.StatusCode);
        }

        #endregion

        #region New Negative Path

        #region Create

        [TestMethod]
        public async Task Create_WhenStudentNotFound_Return400()
        {
            //Arrange 
            var course = builder.CreateAndReturnCourse();

            var enrollmentRequest = new CreateEnrollmentDto
            {
                CourseId = course!.Id
            };

            //Act
            var resposne = await _client.PostAsJsonAsync("/api/enrollment", enrollmentRequest);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, resposne.StatusCode);
        }

        [TestMethod]
        public async Task Create_WhenCourseNotFound_Return400()
        {
            //Arrange 
            var student = builder.CreateAndReturnStudent();

            var enrollmentRequest = new CreateEnrollmentDto
            {
                StudentId = student!.Id,
            };

            //Act
            var resposne = await _client.PostAsJsonAsync("/api/enrollment", enrollmentRequest);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, resposne.StatusCode);
        }
        [TestMethod]
        public async Task Create_WhenEnrollmentDuplicate_Return400()
        {
            //Arrange 

            var student = await builder.CreateAndReturnStudent();
            var course = await builder.CreateAndReturnCourse();

            var enrollmentRequest = new CreateEnrollmentDto
            {
                StudentId = student!.StudentId,
                CourseId = course!.CourseId
            };
            await _client.PostAsJsonAsync("/api/enrollment", enrollmentRequest);

            var request2 = new CreateEnrollmentDto
            {
                StudentId = student!.StudentId,
                CourseId = course!.CourseId
            };

            //Act
            var response = await _client.PostAsJsonAsync("/api/enrollment", request2);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Create_WhenBusinessLogicFails_Return400()
        {
            //Arrange 

            var student = await builder.CreateAndReturnStudent();
            var course = await builder.CreateAndReturnCourse();

            var enrollmentRequest = new CreateEnrollmentDto
            {
                StudentId = student!.StudentId,
                CourseId = course!.CourseId
            };

            //Act 
            var response = await _client.PostAsJsonAsync("/api/enrollment", enrollmentRequest);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        [TestMethod]
        public async Task GetById_WhenEnrollmentNotFound_Return404()
        {

            //Act 
            var resposne = await _client.GetAsync($"/api/enrollment/{7777418}");

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, resposne.StatusCode);

        }

        [TestMethod]
        public async Task Update_WhenRouteIdAndBodyIdMissMatched_Return400()
        {
            //Arrange
            var student = await builder.CreateAndReturnStudent();
            var course = await builder.CreateAndReturnCourse();

            var enrollment = await builder.CreateAndReturnEnrollment(student.StudentId, course.CourseId);

            var update = new UpdateEnrollmentDto
            {
                EnrollmentId = 9999,
                CourseId = course.CourseId,
                StudentId = student.StudentId,
                CancellationReason = " Tesitng "
            };

            //Act 
            var response = await _client.PutAsJsonAsync($"/api/enrollment/{enrollment!.EnrollmentId}", update);
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenEnrollmentDoesNotExists_Return400()
        {
            //Arrange
            var update = new UpdateEnrollmentDto
            {
                CancellationReason = "testing ",
                CancelledDate = DateTimeOffset.UtcNow,
                IsActive = true
            };

            //Act 
            var resposne = await _client.PutAsJsonAsync($"/api/enrollment/{99999}", update);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, resposne.StatusCode);
        }

        [TestMethod]
        public async Task Delete_WhenEnrollmentDoesnotExists_Return404()
        {
            //Act 
            var resposne = await _client.DeleteAsync($"/api/enrollment/{999999944550}");
            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, resposne.StatusCode);
        }

        #endregion
    }
}
