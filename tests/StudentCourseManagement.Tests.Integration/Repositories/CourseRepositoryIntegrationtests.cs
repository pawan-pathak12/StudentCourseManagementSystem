using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper;
using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Common.Builders;
using System.Transactions;

namespace StudentCourseManagement.Tests.Integration.Repositories
{
    [TestClass]
    [DoNotParallelize]
    public class CourseRepositoryIntegrationtests
    {
        private readonly CourseRepository _repository;
        private readonly StudentRepository _studentRepository;
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

        public CourseRepositoryIntegrationtests()
        {
            var fixture = new DatabaseFixture();
            var loggerMock = new Mock<ILogger<CourseRepository>>();
            var loggerMockStudent = new Mock<ILogger<StudentRepository>>();
            var loggerMockEnrollment = new Mock<ILogger<EnrollmentRepository>>();
            _repository = new CourseRepository(fixture.DbContext, loggerMock.Object);
            _studentRepository = new StudentRepository(fixture.DbContext, loggerMockStudent.Object);
            _enrollmentRepository = new EnrollmentRepository(fixture.DbContext, loggerMockEnrollment.Object);
        }
        #region CURD Operations 

        [TestMethod]
        public async Task AddAsync_WithValidCourse_InsertData()
        {
            //act 
            var courseId = await CreateCourseAsync();

            //assert 
            Assert.IsGreaterThan(0, courseId);
            Assert.AreNotEqual(0, courseId);
        }

        [TestMethod]
        public async Task GetAllAsync_IfNotNullThrn_ReturnsListOfCourses()
        {
            //Arrange 
            await CreateCourseAsync();
            //Act
            var courses = await _repository.GetAllAsync();

            //Assert
            Assert.IsNotNull(courses);
            Assert.IsTrue(courses.Any());
        }

        [TestMethod]
        public async Task GetByIdAsync_WithExistingId_ReturnsCourse()
        {
            var courseId = await CreateCourseAsync();

            var course = await _repository.GetByIdAsync(courseId);

            Assert.IsNotNull(course);
        }

        [TestMethod]
        public async Task UpdateAsync_WithExistingId_ReturnsTrue()
        {
            //Aarrange 
            var courseId = await CreateCourseAsync();
            var updateCourseData = new CourseBuilder()
                .WithCode("CS0001").WithInstructor("Hello WOrld")
                .Build();
            //Act

            var isUpdated = await _repository.UpdateAsync(courseId, updateCourseData);
            //Assert
            Assert.IsTrue(isUpdated);

            var course = await _repository.GetByIdAsync(courseId);
            Assert.AreEqual(updateCourseData.Code, course?.Code);
            Assert.AreEqual(updateCourseData.Instructor, course?.Instructor);
        }

        [TestMethod]
        public async Task DeleteAsync_WithActiveExistingCourseId_ReturnTrue()
        {
            ///Arrange 
            var courseId = await CreateCourseAsync();

            //Act 
            var isDeleted = await _repository.DeleteAsync(courseId);

            //Assert
            Assert.IsTrue(isDeleted);
            var course = await _repository.GetByIdAsync(courseId);
            Assert.IsNull(course);
        }
        #endregion

        #region Course Validation 

        [TestMethod]
        public async Task CodeExistsAsync_IfCodeExists_ReturnTrue()
        {
            //Arrange 
            var courseId = await CreateCourseAsync();

            var course = await _repository.GetByIdAsync(courseId);
            //Act 
            var codeExists = await _repository.CodeExistsAsync(course.Code);

            //Assert
            Assert.IsTrue(codeExists);

        }

        [TestMethod]
        public async Task TitleExistsAsync_IfExists_ReturnTrue()
        {
            //Arrange 
            int courseId = await CreateCourseAsync();
            var course = await _repository.GetByIdAsync(courseId);

            //Act
            var titleExists = await _repository.TitleExistsAsync(course.Title);

            //Assert
            Assert.IsTrue(titleExists);
        }
        #endregion

        #region Enrollment required method 

        [TestMethod]
        public async Task CheckEnrollmentDateAsync_WithEnrollmentDateOut_ReturnsFalse()
        {
            //Arrange 
            var courseId = await CreateCourseAsync();
            var testDate = DateTimeOffset.UtcNow.AddMonths(4);
            //Act 
            var isValid = await _repository.CheckEnrollmentDateAsync(courseId, testDate);

            //Assert
            Assert.IsFalse(isValid);

        }
        #endregion

        #region Phase 5 : required Method 
        [TestMethod]
        public async Task GetStartDateByEnrollmentIdAsync_WithExistingEnrollment_ReturnCourseStartDate()
        {
            //Arrange 
            var studentId = await CreateStudentAsync();
            var courseId = await CreateCourseAsync();

            var enrollmentId = await _enrollmentRepository.AddAsync(new Enrollment
            { StudentId = studentId, CourseId = courseId, IsActive = true });

            //Act 
            var startDate = await _repository.GetStartDateByEnrollmentIdAsync(enrollmentId);
            //Assert 
            Assert.IsNotNull(startDate);
        }

        #endregion

        #region Private Helper Methods 

        private async Task<int> CreateCourseAsync()
        {
            var course = new CourseBuilder()
                .Build();
            return await _repository.AddAsync(course);
        }

        private async Task<int> CreateStudentAsync()
        {
            var student = new StudentBuilder()
                .Build();
            return await _studentRepository.AddAsync(student);
        }
        #endregion
    }
}
