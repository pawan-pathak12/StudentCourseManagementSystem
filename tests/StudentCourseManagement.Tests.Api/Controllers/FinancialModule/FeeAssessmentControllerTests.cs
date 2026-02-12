using Microsoft.Extensions.DependencyInjection;
using StudentCourseManagement.Application.DTOs.Courses;
using StudentCourseManagement.Application.DTOs.Enrollments;
using StudentCourseManagement.Application.DTOs.FInancialModule.FeeAssessments;
using StudentCourseManagement.Application.DTOs.Students;
using StudentCourseManagement.Domain.Enums;
using StudentCourseManagement.Tests.Api.Builders;
using System.Net;
using System.Net.Http.Json;

namespace StudentCourseManagement.Tests.Api.Controllers.FinancialModule
{
    [TestClass]
    public class FeeAssessmentControllerTest : IntegrationTestBase
    {
        #region Happy Part 
        [TestMethod]
        public async Task AssessFee_WhenAllBusinessRuleValid_Return200()
        {
            //Arrange 

            //Act 

            //Assert
        }

        [TestMethod]
        public async Task GetAll_WhenFeeAssessmentExists_Return200()
        {
            //Arrange 

            //Act 

            //Assert
        }

        [TestMethod]
        public async Task GetById_WhenFeeAssessmentExists_Return200()
        {
            //Arrange 

            //Act 

            //Assert
        }

        [TestMethod]
        public async Task Update_WhenFeeAssessmentExists_Return200()
        {
            //Arrange 

            //Act 

            //Assert
        }

        [TestMethod]
        public async Task Delete_WhenFeeAssessmentExists_Return204()
        {
            //Arrange 

            //Act 

            //Assert
        }


        #endregion

        #region Negative Part 

        [TestMethod]
        public async Task AssessFee_WhenEnrollmentDoesNotExists_Return400()
        {
            //Arrange 
            int id = 13933;
            //Act 
            var response = await _client.PostAsJsonAsync($"/api/feeAssessment/assess/{id}", id);
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task AssessFee_WhenAnyBusinessRuleFailed_Return400()
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
            var enrollment = await enrollmentResposne.Content
                    .ReadFromJsonAsync<EnrollmentResponseDto>();

            await _client.PutAsJsonAsync($"api/enrollment/{enrollment!.EnrollmentId}", new UpdateEnrollmentDto
            {
                EnrollmentId = enrollment!.EnrollmentId,
                CourseId = enrollment!.CourseId,
                StudentId = enrollment!.StudentId,
                EnrollmentStatus = EnrollmentStatus.Cancelled
            });
            // here enrollment status is cancelled so this must fail 

            //Act 
            var response = await _client.PostAsJsonAsync($"/api/feeAssessment/assess{enrollment!.EnrollmentId}", enrollment.EnrollmentId);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_WhenFeeAssessmentDoesNotExists_Return400()
        {

            //Act 
            var response = await _client.GetAsync($"/api/feeAssessment/{77888881}");

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenFeeAssessmentNotFound_Return400()
        {
            //Arrange 
            int id = 11444;
            var update = new UpdateFeeAssessmentDto
            {
                FeeAssessmentId = id,
                FeeAssessmentStatus = AssessmentStatus.Assessed,
                LateFeeAmount = 0
            };
            //Act 
            var response = await _client.PutAsJsonAsync($"/api/feeAssessment/{id}", update);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenRouteIdAndBodyIdMissMatched_Return400()
        {
            //Arrange 
            var scope = Factory.Services.CreateScope();
            var dataBuilder = new TestDataBuilder(scope.ServiceProvider);

            var student = await dataBuilder.CreateStudent();
            var course = await dataBuilder.CreateCourse();
            var enrollment = await dataBuilder.CreateEnrollment(student!.StudentId, course!.CourseId);
            var feeTemplate = await dataBuilder.CreateFeeTemplate(course.CourseId);

            var feeAssessment = await dataBuilder.CreateFeeAssessment(enrollment!.EnrollmentId, feeTemplate!.FeeTemplateId);

            var update = new UpdateFeeAssessmentDto
            {
                FeeAssessmentId = feeAssessment!.FeeAssessmentId,
                LateFeeAmount = 0,
                DueDate = DateTimeOffset.UtcNow.AddMonths(2)
            };
            //Act 
            var response = await _client.PutAsJsonAsync($"/api/feeAssessment/{245398434}", update);
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_WhenFeeAssessmentNotFound_Return400()
        {

            //Act 
            var resposne = await _client.DeleteAsync($"/api/feeAssessment/{8353485}");

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, resposne.StatusCode);
        }


        #endregion

    }
}
