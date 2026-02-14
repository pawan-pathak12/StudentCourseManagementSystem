using StudentCourseManagement.Application.DTOs.FInancialModule.FeeAssessments;
using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Enums;
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
            var student = await builder.CreateStudent();
            var course = await builder.CreateCourse();
            var feeTemplate = await builder.CreateFeeTemplate(course!.CourseId);
            var enrollment = await builder.CreateEnrollment(student.StudentId, course.CourseId);

            //Act 
            var response = await _client.PostAsJsonAsync($"/api/feeAssessment/assess/{enrollment!.EnrollmentId}", enrollment!.EnrollmentId);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public async Task GetAll_WhenFeeAssessmentExists_Return200()
        {
            //Arrange 
            var student = await builder.CreateStudent();
            var course = await builder.CreateCourse();
            var feeTemplate = await builder.CreateFeeTemplate(course!.CourseId);
            var enrollment = await builder.CreateEnrollment(student.StudentId, course.CourseId);


            var feeAssessment = await builder.CreateFeeAssessment(enrollment!.EnrollmentId, feeTemplate!.FeeTemplateId);

            //Act 
            var response = await _client.GetAsync("/api/feeAssessment/");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetById_WhenFeeAssessmentExists_Return200()
        {
            //Arrange 
            var student = await builder.CreateStudent();
            var course = await builder.CreateCourse();
            var feeTemplate = await builder.CreateFeeTemplate(course!.CourseId);
            var enrollment = await builder.CreateEnrollment(student.StudentId, course.CourseId);


            var feeAssessment = await builder.CreateFeeAssessment(enrollment!.EnrollmentId, feeTemplate!.FeeTemplateId);

            //Act 
            var response = await _client.GetAsync($"/api/feeAssessment/{feeAssessment!.FeeAssessmentId}");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Update_WhenFeeAssessmentExists_Return200()
        {
            //Arrange 
            var student = await builder.CreateStudent();
            var course = await builder.CreateCourse();
            var feeTemplate = await builder.CreateFeeTemplate(course!.CourseId);
            var enrollment = await builder.CreateEnrollment(student.StudentId, course.CourseId);


            var feeAssessment = await builder.CreateFeeAssessment(enrollment!.EnrollmentId, feeTemplate!.FeeTemplateId);

            var updateData = new UpdateFeeAssessmentDto
            {
                FeeAssessmentId = feeAssessment.FeeAssessmentId,
                FeeAssessmentStatus = AssessmentStatus.Assessed,
                Amount = 00,
                PaidDate = DateTimeOffset.UtcNow
            };

            //Act 
            var response = await _client.PutAsJsonAsync($"/api/feeAssessment/{feeAssessment.FeeAssessmentId}", updateData);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_WhenFeeAssessmentExists_Return204()
        {
            //Arrange 
            var student = await builder.CreateStudent();
            var course = await builder.CreateCourse();
            var feeTemplate = await builder.CreateFeeTemplate(course!.CourseId);
            var enrollment = await builder.CreateEnrollment(student.StudentId, course.CourseId);


            var feeAssessment = await builder.CreateFeeAssessment(enrollment!.EnrollmentId, feeTemplate!.FeeTemplateId);

            //Act 
            var response = await _client.DeleteAsync($"/api/feeAssessment/{feeAssessment!.FeeAssessmentId}");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
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
            var student = await builder.CreateStudent();
            var course = await builder.CreateCourse();

            var enrollment = await builder.CreateEnrollment(student.StudentId, course.CourseId);

            var updateEnrollment = new Enrollment
            {
                EnrollmentId = enrollment!.EnrollmentId,
                CourseId = enrollment!.CourseId,
                StudentId = enrollment!.StudentId,
                EnrollmentStatus = EnrollmentStatus.Cancelled
            };

            await builder.UpdateEnrollment(enrollment.EnrollmentId, updateEnrollment);
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
            var student = await builder.CreateStudent();
            var course = await builder.CreateCourse();
            var enrollment = await builder.CreateEnrollment(student!.StudentId, course!.CourseId);
            var feeTemplate = await builder.CreateFeeTemplate(course.CourseId);

            var feeAssessment = await builder.CreateFeeAssessment(enrollment!.EnrollmentId, feeTemplate!.FeeTemplateId);

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
