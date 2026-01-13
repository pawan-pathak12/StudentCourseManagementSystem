using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Tests.Unit.Common;

namespace StudentCourseManagement.Tests.Unit.Services.AcademicModule.Enrollments
{
    [TestClass]
    public class EnrollmentServiceTests_Create : EnrollmentServiceTestBase
    {

        [TestMethod]
        public async Task CreateAsync_WithNewLogic_CreateEnrollment()
        {
            var student = new Student
            {
                Address = "testing"
            };

            var course = new Course
            {
                Capacity = 1
            };

            var enrollment = new Enrollment
            {
                StudentId = 1,
                CourseId = 1
            };

            await _studentRepository.AddAsync(student);
            await _courseRepository.AddAsync(course);

            var isCreated = await _service.CreateAsync(enrollment);

            Assert.IsTrue(isCreated);
        }

        [TestMethod]
        public async Task CreateAsync_WhenEnrollmentAlreadyExists_Returnsfalse()
        {
            //Arrange 
            var student = new Student
            {
                Address = "testing"
            };

            var course = new Course
            {
                Capacity = 1
            };

            await _studentRepository.AddAsync(student);
            await _courseRepository.AddAsync(course);

            var enrollment = new Enrollment
            {
                StudentId = 1,
                CourseId = 1,
            };

            await _service.CreateAsync(enrollment);

            var enrollment2 = new Enrollment
            {
                StudentId = 1,
                CourseId = 1,
            };

            //Act 
            var isCreated = await _service.CreateAsync(enrollment2);

            //Assert 

            Assert.IsFalse(isCreated);

        }

        [TestMethod]
        public async Task CreateAsync_WhenCourseIsFull_ReturnsFalse()
        {
            //Arrange 
            var student = new Student
            {
                Address = "testing"
            };

            var student2 = new Student
            {
                Name = "Ram",
                Address = "Btm"
            };
            var course = new Course
            {
                Capacity = 1
            };

            await _studentRepository.AddAsync(student2);
            await _studentRepository.AddAsync(student);

            await _courseRepository.AddAsync(course);

            var enrollment = new Enrollment
            {
                StudentId = 1,
                CourseId = 1,
            };

            await _service.CreateAsync(enrollment);

            var enrollment2 = new Enrollment
            {
                StudentId = 2,
                CourseId = 1,
            };

            //Act 
            var isCreated = await _service.CreateAsync(enrollment2);

            //Assert 

            Assert.IsFalse(isCreated);
        }

        [TestMethod]
        public async Task CreateAsync_WhenStudentHasReachedMaxEnrollments_ReturnsFalse()
        {
            //Arrange 

            #region Student 
            var student1 = new Student
            {
                Name = "Sita",
                Address = "Dharan",
                IsActive = true
            };

            var student2 = new Student
            {
                Name = "Hari",
                Address = "Itahari",
                IsActive = true
            };

            var student3 = new Student
            {
                Name = "Gita",
                IsActive = true,
                Address = "Biratnagar"
            };

            var student4 = new Student
            {
                Name = "Krishna",
                IsActive = true,
                Address = "Jhapa"
            };
            var student5 = new Student
            {
                Name = "Ram ",
                IsActive = true,
                Address = "Birtamode"
            };

            await _studentRepository.AddAsync(student1);
            await _studentRepository.AddAsync(student2);
            await _studentRepository.AddAsync(student3);
            await _studentRepository.AddAsync(student4);
            await _studentRepository.AddAsync(student5);



            #endregion

            #region Course 
            var course1 = new Course
            {
                Title = "Database Management Systems",
                Capacity = 25,
                IsActive = true
            };

            var course2 = new Course
            {
                Title = "Digital Logic Design",
                Capacity = 30,
                IsActive = true
            };

            var course3 = new Course
            {
                Title = "Web Development Fundamentals",
                Capacity = 20,
                IsActive = true
            };

            var course4 = new Course
            {
                Title = "Artificial Intelligence Basics",
                Capacity = 15,
                IsActive = true
            };

            var course5 = new Course
            {
                Title = "C Programming master class",
                Capacity = 11,
                IsActive = true
            };

            await _courseRepository.AddAsync(course1);
            await _courseRepository.AddAsync(course2);
            await _courseRepository.AddAsync(course3);
            await _courseRepository.AddAsync(course4);
            await _courseRepository.AddAsync(course5);


            #endregion

            //Act and Assert 

            for (int i = 1; i <= 6; i++)
            {
                var enrollment = new Enrollment { StudentId = 2, CourseId = i };
                var isCreated = await _service.CreateAsync(enrollment);

                if (i < 6)
                {
                    Assert.IsTrue(isCreated);
                }
                else
                {
                    Assert.IsFalse(isCreated);
                }
            }

        }

        [TestMethod]
        public async Task CreateAsync_WhenEnrollmentDateWindowIsOut_ReturnFalse()
        {
            //Arrange 
            var student = new Student
            {
                Name = "Ram",
                Address = "Ktm"
            };

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
            var isCreated = await _service.CreateAsync(enrollment);

            Assert.IsFalse(isCreated);
        }
    }
}
