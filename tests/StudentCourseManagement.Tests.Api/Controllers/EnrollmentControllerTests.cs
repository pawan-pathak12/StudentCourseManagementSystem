using StudentCourseManagement.Application.DTOs.Courses;
using StudentCourseManagement.Application.DTOs.Enrollments;
using StudentCourseManagement.Application.DTOs.Students;
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
        [TestMethod]
        public async Task Create_WhenStudentNotFound_Return404()
        {
            //Arrange 

            //Act

            //Assert
        }

        [TestMethod]
        public async Task Create_WhenCourseNotFound_Return404()
        {
            //Arrange 

            //Act

            //Assert
        }
        [TestMethod]
        public async Task Create_WhenEnrollmentDuplicate_Return400()
        {
            //Arrange 

            //Act

            //Assert
        }

        [TestMethod]
        public async Task Create_WhenBusinessLogicFails_Return400()
        {
            //Arrange 

            //Act

            //Assert
        }


        #endregion




    }
}
