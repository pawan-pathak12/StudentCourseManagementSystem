using StudentCourseManagement.Application.DTOs.Courses;
using StudentCourseManagement.Application.DTOs.Enrollments;
using StudentCourseManagement.Application.DTOs.Students;
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
            var createStudent = new CreateStudentDto
            {
                Name = "Pawan",
                Address = "Haldibari"
            };
            var studentResponse = await _client
                .PostAsJsonAsync("/api/student", createStudent);
            var student = await studentResponse.Content
                .ReadFromJsonAsync<StudentResponseDto>();

            var createCourse = new CreateCourseDto
            {
                Title = "Math",
                Credits = 3
            };
            var courseResponse = await _client
                .PostAsJsonAsync("/api/course", createCourse);
            var course = await courseResponse.Content
                .ReadFromJsonAsync<CourseResponseDto>();

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

            var createStudent = new CreateStudentDto
            {
                Name = "Pawan",
                Address = "Haldibari"
            };
            var studentResponse = await _client
                .PostAsJsonAsync("/api/student", createStudent);
            var student = await studentResponse.Content
                .ReadFromJsonAsync<StudentResponseDto>();

            var createCourse = new CreateCourseDto
            {
                Title = "Math",
                Credits = 3
            };
            var courseResponse = await _client
                .PostAsJsonAsync("/api/course", createCourse);
            var course = await courseResponse.Content
                .ReadFromJsonAsync<CourseResponseDto>();

            var enrollmentRequest = new CreateEnrollmentDto
            {
                StudentId = student!.StudentId,
                CourseId = course!.CourseId
            };

            var enrollmentResponse = await _client.PostAsJsonAsync("/api/enrollment", enrollmentRequest);
            var enrollment = await enrollmentResponse.Content
                .ReadFromJsonAsync<EnrollmentResponseDto>();

            //Act
            var existingEnrollment = await _client.GetAsync($"/api/enrollment/{enrollment!.EnrollmentId}");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, existingEnrollment.StatusCode);

        }
        [TestMethod]
        public async Task GetAll_WhenEnrollmentExists_Return200()
        {
            //Arrange 

            var createStudent = new CreateStudentDto
            {
                Name = "Pawan",
                Address = "Haldibari"
            };
            var studentResponse = await _client
                .PostAsJsonAsync("/api/student", createStudent);
            var student = await studentResponse.Content
                .ReadFromJsonAsync<StudentResponseDto>();

            var createCourse = new CreateCourseDto
            {
                Title = "Math",
                Credits = 3
            };
            var courseResponse = await _client
                .PostAsJsonAsync("/api/course", createCourse);
            var course = await courseResponse.Content
                .ReadFromJsonAsync<CourseResponseDto>();

            var enrollmentRequest = new CreateEnrollmentDto
            {
                StudentId = student!.StudentId,
                CourseId = course!.CourseId
            };

            await _client.PostAsJsonAsync("/api/enrollment", enrollmentRequest);

            //Act
            var existingEnrollment = await _client.GetAsync("/api/enrollment/");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, existingEnrollment.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenEnrollmentExists_Return204()
        {
            //Arrange 

            var createStudent = new CreateStudentDto
            {
                Name = "Pawan",
                Address = "Haldibari"
            };
            var studentResponse = await _client
                .PostAsJsonAsync("/api/student", createStudent);
            var student = await studentResponse.Content
                .ReadFromJsonAsync<StudentResponseDto>();

            var createCourse = new CreateCourseDto
            {
                Title = "Math",
                Credits = 3
            };
            var courseResponse = await _client
                .PostAsJsonAsync("/api/course", createCourse);
            var course = await courseResponse.Content
                .ReadFromJsonAsync<CourseResponseDto>();

            var enrollmentRequest = new CreateEnrollmentDto
            {
                StudentId = student!.StudentId,
                CourseId = course!.CourseId
            };
            var enrollmentResponse = await _client.PostAsJsonAsync("/api/enrollment", enrollmentRequest);
            var enrollment = await enrollmentResponse.Content
                .ReadFromJsonAsync<EnrollmentResponseDto>();

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

            var createStudent = new CreateStudentDto
            {
                Name = "Pawan",
                Address = "Haldibari"
            };
            var studentResponse = await _client
                .PostAsJsonAsync("/api/student", createStudent);
            var student = await studentResponse.Content
                .ReadFromJsonAsync<StudentResponseDto>();

            var createCourse = new CreateCourseDto
            {
                Title = "Math",
                Credits = 3
            };
            var courseResponse = await _client
                .PostAsJsonAsync("/api/course", createCourse);
            var course = await courseResponse.Content
                .ReadFromJsonAsync<CourseResponseDto>();

            var enrollmentRequest = new CreateEnrollmentDto
            {
                StudentId = student!.StudentId,
                CourseId = course!.CourseId
            };

            var enrollmentResponse = await _client.PostAsJsonAsync("/api/enrollment", enrollmentRequest);
            var enrollment = await enrollmentResponse.Content
                .ReadFromJsonAsync<EnrollmentResponseDto>();

            //Act
            var existingEnrollment = await _client.DeleteAsync($"/api/enrollment/{enrollment!.EnrollmentId}");

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, existingEnrollment.StatusCode);
        }

        #endregion

        #region New Negative Path

        #region Create

        [TestMethod]
        public async Task Create_WhenStudentNotFound_Return404()
        {
            //Arrange 
            var createCourse = new CreateCourseDto
            {
                Title = "Math",
                Credits = 3
            };
            var courseResponse = await _client
                .PostAsJsonAsync("/api/course", createCourse);
            var course = await courseResponse.Content
                .ReadFromJsonAsync<CourseResponseDto>();

            var enrollmentRequest = new CreateEnrollmentDto
            {
                CourseId = course!.CourseId
            };

            //Act
            var resposne = await _client.PostAsJsonAsync("/api/enrollment", enrollmentRequest);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, resposne.StatusCode);
        }

        [TestMethod]
        public async Task Create_WhenCourseNotFound_Return404()
        {
            //Arrange 

            var createStudent = new CreateStudentDto
            {
                Name = "Pawan",
                Address = "Haldibari"
            };
            var studentResponse = await _client
                .PostAsJsonAsync("/api/student", createStudent);
            var student = await studentResponse.Content
                .ReadFromJsonAsync<StudentResponseDto>();


            var enrollmentRequest = new CreateEnrollmentDto
            {
                StudentId = student!.StudentId,
            };

            //Act
            var resposne = await _client.PostAsJsonAsync("/api/enrollment", enrollmentRequest);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, resposne.StatusCode);
        }
        [TestMethod]
        public async Task Create_WhenEnrollmentDuplicate_Return400()
        {
            //Arrange 

            var createStudent = new CreateStudentDto
            {
                Name = "Pawan",
                Address = "Haldibari"
            };
            var studentResponse = await _client
                .PostAsJsonAsync("/api/student", createStudent);
            var student = await studentResponse.Content
                .ReadFromJsonAsync<StudentResponseDto>();

            var createCourse = new CreateCourseDto
            {
                Title = "Math",
                Credits = 3
            };
            var courseResponse = await _client
                .PostAsJsonAsync("/api/course", createCourse);
            var course = await courseResponse.Content
                .ReadFromJsonAsync<CourseResponseDto>();

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

            var createStudent = new CreateStudentDto
            {
                Name = "Pawan",
                Address = "Haldibari"
            };
            var studentResponse = await _client
                .PostAsJsonAsync("/api/student", createStudent);
            var student = await studentResponse.Content
                .ReadFromJsonAsync<StudentResponseDto>();

            var createCourse = new CreateCourseDto
            {
                Title = "Math",
                Credits = 3
            };
            var courseResponse = await _client
                .PostAsJsonAsync("/api/course", createCourse);
            var course = await courseResponse.Content
                .ReadFromJsonAsync<CourseResponseDto>();

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
            var createStudent = new CreateStudentDto
            {
                Name = "Pawan",
                Address = "Haldibari"
            };
            var studentResponse = await _client
                .PostAsJsonAsync("/api/student", createStudent);
            var student = await studentResponse.Content
                .ReadFromJsonAsync<StudentResponseDto>();

            var createCourse = new CreateCourseDto
            {
                Title = "Math",
                Credits = 3
            };
            var courseResponse = await _client
                .PostAsJsonAsync("/api/course", createCourse);
            var course = await courseResponse.Content
                .ReadFromJsonAsync<CourseResponseDto>();

            var enrollmentRequest = new CreateEnrollmentDto
            {
                StudentId = student!.StudentId,
                CourseId = course!.CourseId
            };

            var enrollmentResposne = await _client.PostAsJsonAsync("/api/enrollment", enrollmentRequest);
            var createdEnrollment = await enrollmentResposne.Content
                .ReadFromJsonAsync<EnrollmentResponseDto>();

            var update = new UpdateEnrollmentDto
            {
                EnrollmentId = 9999,
                CourseId = course.CourseId,
                StudentId = student.StudentId,
                CancellationReason = " Tesitng "
            };

            //Act 

            var response = await _client.PutAsJsonAsync($"/api/enrollment/{createdEnrollment!.EnrollmentId}", update);
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenEnrollmentDoesnotExists_Return404()
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
            Assert.AreEqual(HttpStatusCode.NotFound, resposne.StatusCode);
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
