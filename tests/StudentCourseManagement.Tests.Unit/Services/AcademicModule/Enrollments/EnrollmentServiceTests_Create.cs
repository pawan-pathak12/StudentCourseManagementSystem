using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Common.Builders;
using StudentCourseManagement.Tests.Unit.Common;

namespace StudentCourseManagement.Tests.Unit.Services.AcademicModule.Enrollments
{
    [TestClass]
    public class EnrollmentServiceTests_Create : EnrollmentServiceTestBase
    {

        [TestMethod]
        public async Task CreateAsync_WithValidData_ReturnsTrue()
        {
            //Arrange 
            var student = new StudentBuilder()
                .Build();
            var course = new CourseBuilder()
                   .Build();

            var studentId = await _studentRepository.AddAsync(student);
            var courseId = await _courseRepository.AddAsync(course);

            var enrollment = new EnrollmentBuilder()
                .WithStudentId(studentId).WithCourseId(courseId).Build();
            //Act 

            var (success, errorMessage, enrollmentId) = await _service.CreateAsync(enrollment);
            //Assert 
            Assert.IsTrue(success);
            Assert.IsTrue(enrollmentId > 0);
        }

        [TestMethod]
        public async Task CreateAsync_WhenEnrollmentAlreadyExists_Returnsfalse()
        {
            //Arrange 
            var student = new StudentBuilder()
                 .Build();
            var course = new CourseBuilder()
                   .Build();

            var studentId = await _studentRepository.AddAsync(student);
            var courseId = await _courseRepository.AddAsync(course);

            var enrollment = new EnrollmentBuilder()
                          .WithStudentId(studentId).WithCourseId(courseId).Build();

            await _service.CreateAsync(enrollment);

            var enrollment2 = new EnrollmentBuilder()
                            .WithStudentId(studentId).WithCourseId(courseId).Build();

            //Act 
            var (success, errorMessage, enrollmentId) = await _service.CreateAsync(enrollment2);

            //Assert 
            Assert.IsFalse(success);
            Assert.IsNotNull(errorMessage);

        }

        [TestMethod]
        public async Task CreateAsync_WhenCourseIsFull_ReturnsFalse()
        {
            //Arrange 
            var student = new StudentBuilder()
               .Build();
            var student2 = new StudentBuilder()
                .WithName("Pawan").Build();

            var studentId = await _studentRepository.AddAsync(student);
            var secondStudentId = await _studentRepository.AddAsync(student2);

            var course = new CourseBuilder()
            .WithCapacity(1).Build();
            var courseId = await _courseRepository.AddAsync(course);

            var enrollment = new EnrollmentBuilder()
                .WithStudentId(studentId).WithCourseId(courseId).Build();

            await _service.CreateAsync(enrollment);

            var enrollment2 = new EnrollmentBuilder()
               .WithStudentId(secondStudentId).WithCourseId(courseId).Build();


            //Act 
            var (success, errorMessage, enrollmentId) = await _service.CreateAsync(enrollment2);

            //Assert 

            Assert.IsFalse(success);
        }

        [TestMethod]
        public async Task CreateAsync_WhenStudentHasReachedMaxEnrollments_ReturnsFalse()
        {
            //Arrange 

            #region Student 

            var student1 = new StudentBuilder()
                         .WithName("Ram")
                         .Build();

            var student2 = new StudentBuilder()
                        .WithName("Shyam")
                        .Build();

            var student3 = new StudentBuilder()
                        .WithName("Hari")
                        .Build();

            var student4 = new StudentBuilder()
                        .WithName("Gita")
                        .Build();

            var student5 = new StudentBuilder()
                        .WithName("Sita")
                        .Build();


            var firstStudentId = await _studentRepository.AddAsync(student1);
            await _studentRepository.AddAsync(student2);
            await _studentRepository.AddAsync(student3);
            await _studentRepository.AddAsync(student4);
            await _studentRepository.AddAsync(student5);


            #endregion

            #region Course 
            var course1 = new CourseBuilder()
                          .WithTitle("Introduction to Programming")
                          .WithStartDate(DateTimeOffset.UtcNow.AddDays(40))
                        .WithEndDate(DateTimeOffset.UtcNow.AddMonths(2))
                        .WithEnrollmentStartDate(DateTimeOffset.UtcNow.AddDays(-5))
                        .WithEnrollmentEndDate(DateTimeOffset.UtcNow.AddDays(35))
                          .Build();

            var course2 = new CourseBuilder()
                        .WithTitle("Database Systems")
                       .WithStartDate(DateTimeOffset.UtcNow.AddDays(45))
                        .WithEndDate(DateTimeOffset.UtcNow.AddMonths(12))
                        .WithEnrollmentStartDate(DateTimeOffset.UtcNow.AddDays(-15))
                        .WithEnrollmentEndDate(DateTimeOffset.UtcNow.AddDays(40))
                        .Build();

            var course3 = new CourseBuilder()
                        .WithTitle("Web Development Fundamentals")
                         .WithStartDate(DateTimeOffset.UtcNow.AddDays(10))
                        .WithEndDate(DateTimeOffset.UtcNow.AddMonths(2))
                        .WithEnrollmentStartDate(DateTimeOffset.UtcNow.AddDays(-1))
                        .WithEnrollmentEndDate(DateTimeOffset.UtcNow.AddDays(5))
                        .Build();

            var course4 = new CourseBuilder()
                        .WithTitle("Software Engineering Practices")
                        .WithStartDate(DateTimeOffset.UtcNow.AddDays(30))
                        .WithEndDate(DateTimeOffset.UtcNow.AddMonths(3))
                        .WithEnrollmentStartDate(DateTimeOffset.UtcNow.AddDays(-10))
                        .WithEnrollmentEndDate(DateTimeOffset.UtcNow.AddDays(25))
                        .Build();

            var course5 = new CourseBuilder()
                    .WithTitle("Cloud Computing Basics")
                    .WithStartDate(DateTimeOffset.UtcNow.AddDays(40))
                    .WithEndDate(DateTimeOffset.UtcNow.AddMonths(2))
                    .WithEnrollmentStartDate(DateTimeOffset.UtcNow.AddDays(-15))
                    .WithEnrollmentEndDate(DateTimeOffset.UtcNow.AddDays(35))
                    .Build();

            var courseId = new int[6];
            courseId[0] = await _courseRepository.AddAsync(course1);
            courseId[1] = await _courseRepository.AddAsync(course2);
            courseId[2] = await _courseRepository.AddAsync(course3);
            courseId[3] = await _courseRepository.AddAsync(course4);
            courseId[4] = await _courseRepository.AddAsync(course5);


            #endregion

            //Act and Assert 

            for (int i = 0; i < courseId.Length; i++)
            {
                var enrollment = new Enrollment { StudentId = firstStudentId, CourseId = courseId[i], IsActive = true };
                var (success, errorMessage, enrollmentId) = await _service.CreateAsync(enrollment);

                if (i < 5)
                {
                    Assert.IsTrue(success);
                }
                else
                {
                    Assert.IsFalse(success);
                }
            }

        }

        [TestMethod]
        public async Task CreateAsync_WhenEnrollmentDateWindowIsOut_ReturnFalse()
        {
            //Arrange 
            var student = new StudentBuilder()
                          .WithName("Ram")
                          .Build();

            var course = new Course
            {
                Title = "C# Master class",
                Capacity = 11,
                EnrollmentStartDate = new DateTimeOffset(2025, 12, 01, 8, 0, 0, TimeSpan.FromHours(5.75)),
                EnrollmentEndDate = new DateTimeOffset(2025, 12, 15, 17, 0, 0, TimeSpan.FromHours(5.75)),
                IsActive = true
            };

            var studentId = await _studentRepository.AddAsync(student);
            var courseId = await _courseRepository.AddAsync(course);

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = new DateTimeOffset(2025, 11, 25, 10, 0, 0, TimeSpan.FromHours(5.75))
            };

            //Act 
            var (success, errorMessage, enrollmentId) = await _service.CreateAsync(enrollment);

            Assert.IsFalse(success);
        }
    }
}
