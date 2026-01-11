using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper;
using StudentCourseManagement.Domain.Entities;
using System.Transactions;

namespace StudentCourseManagement.Tests.Integration.Repositories
{
    [TestClass]
    public class EnrollmentRepositoryIntegrationTests
    {
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;
        private readonly EnrollmentRepository _enrollmentRepository;
        public EnrollmentRepositoryIntegrationTests()
        {
            var fixture = new DatabaseFixture();

            var mockLoggerStudent = new Mock<ILogger<StudentRepository>>();
            var mockLoggerCourse = new Mock<ILogger<CourseRepository>>();
            var loggerMock = new Mock<ILogger<EnrollmentRepository>>();

            _studentRepository = new StudentRepository(fixture.DbContext, mockLoggerStudent.Object);
            _courseRepository = new CourseRepository(fixture.DbContext, mockLoggerCourse.Object);
            _enrollmentRepository = new EnrollmentRepository(fixture.DbContext, loggerMock.Object);
        }

        #region CRUD Operations 

        [TestMethod]
        public async Task AddAsync_WithValidData_InsertData()
        {
            //Arrange 
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();

            //Act 
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);

            Assert.AreNotEqual(0, enrollmentId);
            Assert.IsNotNull(enrollmentId);
        }

        [TestMethod]
        public async Task GetAll_IfNotNull_ReturnEnrollments()
        {
            //Arrange 
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            await CreateEnrollmentAsync(studentId, courseId);

            var enrollments = await _enrollmentRepository.GetAllAsync();

            //Assert 
            Assert.IsNotNull(enrollments);
            Assert.IsTrue(enrollments.Any());
        }
        [TestMethod]
        public async Task GetAll_WithExistingData_ReturnEnrollment()
        {
            //Arrange 
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);

            //Act
            var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);
            Assert.IsNotNull(enrollment);
        }
        [TestMethod]
        public async Task Update_WithExistingId_ReturnTrue()
        {
            //Arrange 
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);

            //act 
            var updateEnrollment = new Enrollment
            {
                EnrollmentId = enrollmentId,
                StudentId = 1,
                CourseId = 2,
                IsActive = true,
                FeeAssessmentDate = null,
                CancellationReason = " ",
                CancelledDate = DateTimeOffset.UtcNow,
            };

            var isUpdated = await _enrollmentRepository.UpdateAsync(enrollmentId, updateEnrollment);

            //Assert 
            Assert.IsTrue(isUpdated);
            var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);
            Assert.AreEqual(updateEnrollment.CancelledDate, enrollment.CancelledDate);
        }
        [TestMethod]
        public async Task DeleteAsync_WithExistingData_ReturnsFalse()
        {
            //Arrange 
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            //Act 
            var isDeleted = await _enrollmentRepository.DeleteAsync(enrollmentId);

            //Act 
            Assert.IsTrue(isDeleted);
            var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);
            Assert.IsNull(enrollment);

        }

        #endregion

        #region Business Logic Test 

        [TestMethod]
        public async Task ExistsAsync_WithExistingEnrollment_ReturnsTrue()
        {
            //Arrange 
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);

            //Act 

            var doesExists = await _enrollmentRepository.ExistsAsync(studentId, courseId);

            Assert.IsTrue(doesExists);
        }

        [TestMethod]
        public async Task GetEnrollmentCountByCourse_ReturnInteger()
        {
            //Arrange 
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var courseId = await CreateCourseAsync();

            var count = await _enrollmentRepository.GetEnrollmentCountByCourse(courseId);

            //Act 
            Assert.IsTrue(count >= 0);

        }

        [TestMethod]
        public async Task GetEnrollmentCountByStudent_ReturnInteger()
        {
            //Arrange 
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var studentId = await CreateStudentAsync();


            var count = await _enrollmentRepository.GetEnrollmentCountByStudent(studentId);

            Assert.IsTrue(count >= 0);


        }
        #endregion

        #region Private Helper Methods 

        private async Task<int> CreateStudentAsync()
        {
            var student = new Student
            {
                Name = "Sita Sharma",
                Email = "sita.sharma@example.com",
                DOB = new DateTimeOffset(2004, 05, 12, 0, 0, 0, TimeSpan.FromHours(5.75)),
                Number = 9812345678,
                IsActive = true,
                Gender = "Female",
                Address = "Biratnagar, Nepal"
            };
            return await _studentRepository.AddAsync(student);
        }

        private async Task<int> CreateCourseAsync()
        {
            var course = new Course
            {
                Code = "CS1001",
                Title = "Introduction to Programming",
                Credits = 3,
                Description = "Fundamentals of programming using C# and .NET Core.",
                Instructor = "Dr. Anil Sharma",
                StartDate = DateTimeOffset.UtcNow.AddDays(40),
                EndDate = DateTimeOffset.UtcNow.AddMonths(2),
                IsActive = true,
                Capacity = 50,
                EnrollmentStartDate = DateTimeOffset.UtcNow.AddDays(10),
                EnrollmentEndDate = DateTimeOffset.UtcNow.AddDays(25),
            };

            return await _courseRepository.AddAsync(course);
        }
        private async Task<int> CreateEnrollmentAsync(int studentId, int courseId)
        {
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                IsActive = true
            };

            return await _enrollmentRepository.AddAsync(enrollment);

        }
        #endregion


    }
}
