using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper;
using StudentCourseManagement.Tests.Common.Builders;
using System.Transactions;

namespace StudentCourseManagement.Tests.Integration.Repositories
{
    [TestClass]
    [DoNotParallelize]
    public class EnrollmentRepositoryIntegrationTests
    {
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;
        private readonly EnrollmentRepository _enrollmentRepository;
        private TransactionScope _scope;

        [TestInitialize]
        public void Init()
        {
            _scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _scope.Dispose(); // rollback
        }

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
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);

            //act 
            var updateEnrollment = new EnrollmentBuilder()
                            .WithCourseId(courseId).WithStudentId(studentId)
                            .WithEnrollmentId(enrollmentId).WithCancellationReason("nothing")
                            .Build();

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
            //Arrange 
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            //Act
            var count = await _enrollmentRepository.GetEnrollmentCountByCourse(courseId);

            //Assert 
            Assert.IsGreaterThan(0, count);

        }

        [TestMethod]
        public async Task GetEnrollmentCountByStudent_ReturnInteger()
        {
            //Arrange 
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);
            //Act
            var count = await _enrollmentRepository.GetEnrollmentCountByStudent(studentId);
            //Assert
            Assert.IsGreaterThan(0, count);

        }
        #endregion

        #region Phase -3 Required Method 
        [TestMethod]
        public async Task UpdateFeeAssessedDateAsync_WithExistingEnrollmentId_ReturnTrue()
        {
            //Arrange 
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();
            var enrollmentId = await CreateEnrollmentAsync(studentId, courseId);

            //act
            var result = await _enrollmentRepository.UpdateFeeAssessedDateAsync(enrollmentId);

            //Assert
            Assert.IsTrue(result);
        }
        #endregion

        #region Private Helper Methods 

        private async Task<int> CreateStudentAsync()
        {
            var student = new StudentBuilder()
                .Build();

            return await _studentRepository.AddAsync(student);
        }

        private async Task<int> CreateCourseAsync()
        {
            var course = new CourseBuilder()
                  .Build();

            return await _courseRepository.AddAsync(course);
        }
        private async Task<int> CreateEnrollmentAsync(int studentId, int courseId)
        {
            var enrollment = new EnrollmentBuilder()
                .WithStudentId(studentId).WithCourseId(courseId)
                .Build();

            return await _enrollmentRepository.AddAsync(enrollment);

        }
        #endregion


    }
}
