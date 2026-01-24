using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Unit.Common;
using StudentCourseManagement.Tests.Unit.TestUtils.Builders;

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
                          .WithStartDate(new DateTimeOffset(2026, 2, 1, 0, 0, 0, TimeSpan.Zero))
                          .WithEndDate(new DateTimeOffset(2026, 5, 1, 0, 0, 0, TimeSpan.Zero))
                          .WithEnrollmentStartDate(new DateTimeOffset(2026, 1, 15, 0, 0, 0, TimeSpan.Zero))
                          .Build();

            var course2 = new CourseBuilder()
                        .WithTitle("Database Systems")
                        .WithStartDate(new DateTimeOffset(2026, 3, 1, 0, 0, 0, TimeSpan.Zero))
                        .WithEndDate(new DateTimeOffset(2026, 6, 1, 0, 0, 0, TimeSpan.Zero))
                        .WithEnrollmentStartDate(new DateTimeOffset(2026, 2, 10, 0, 0, 0, TimeSpan.Zero))
                        .Build();

            var course3 = new CourseBuilder()
                        .WithTitle("Web Development Fundamentals")
                        .WithStartDate(new DateTimeOffset(2026, 4, 1, 0, 0, 0, TimeSpan.Zero))
                        .WithEndDate(new DateTimeOffset(2026, 7, 1, 0, 0, 0, TimeSpan.Zero))
                        .WithEnrollmentStartDate(new DateTimeOffset(2026, 3, 15, 0, 0, 0, TimeSpan.Zero))
                        .Build();

            var course4 = new CourseBuilder()
                    .WithTitle("Software Engineering Practices")
                    .WithStartDate(new DateTimeOffset(2026, 5, 1, 0, 0, 0, TimeSpan.Zero))
                    .WithEndDate(new DateTimeOffset(2026, 8, 1, 0, 0, 0, TimeSpan.Zero))
                    .WithEnrollmentStartDate(new DateTimeOffset(2026, 4, 10, 0, 0, 0, TimeSpan.Zero))
                    .Build();

            var course5 = new CourseBuilder()
                    .WithTitle("Cloud Computing Basics")
                    .WithStartDate(new DateTimeOffset(2026, 6, 1, 0, 0, 0, TimeSpan.Zero))
                    .WithEndDate(new DateTimeOffset(2026, 9, 1, 0, 0, 0, TimeSpan.Zero))
                    .WithEnrollmentStartDate(new DateTimeOffset(2026, 5, 15, 0, 0, 0, TimeSpan.Zero))
                    .Build();

            var courseId = new int[7];
            courseId[0] = await _courseRepository.AddAsync(course1);
            courseId[1] = await _courseRepository.AddAsync(course2);
            courseId[2] = await _courseRepository.AddAsync(course3);
            courseId[3] = await _courseRepository.AddAsync(course4);
            courseId[4] = await _courseRepository.AddAsync(course5);


            #endregion

            //Act and Assert 

            for (int i = 0; i < courseId.Length; i++)
            {
                var enrollment = new Enrollment { StudentId = firstStudentId, CourseId = courseId[i] };
                var (success, errorMessage, enrollmentId) = await _service.CreateAsync(enrollment);

                if (i < courseId[5])
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
