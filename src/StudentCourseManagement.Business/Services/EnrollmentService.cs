using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Business.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _repository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<EnrollmentService> _logger;
        private static int maxEnrollCount = 5;

        public EnrollmentService(IEnrollmentRepository repository, IStudentRepository studentRepository, ICourseRepository courseRepository, ILogger<EnrollmentService> logger)
        {
            _repository = repository;
            this._studentRepository = studentRepository;
            this._courseRepository = courseRepository;
            this._logger = logger;
        }

        public async Task<(bool success, string? errorMessage, int id)> CreateAsync(Enrollment enrollment)
        {
            var student = await _studentRepository.GetByIdAsync(enrollment.StudentId);
            if (student == null)
            {
                _logger.LogWarning($"Service : Enrollment Failed : Student with Id {enrollment.StudentId} not found or is inactive");
                return (false, $"Student with Id {enrollment.StudentId} not found or is inactive", 0);
            }

            var course = await _courseRepository.GetByIdAsync(enrollment.CourseId);
            if (course == null)
            {
                _logger.LogWarning($"Service : Enrollment Failed : Course with Id {enrollment.CourseId} not found or is inactive");
                return (false, $"Course with Id {enrollment.CourseId} not found or is inactive", 0);
            }

            var enrollmentExists = await _repository.ExistsAsync(enrollment.StudentId, enrollment.CourseId);

            if (enrollmentExists)
            {
                _logger.LogWarning($"Service : Enrollment failed student with Id {enrollment.StudentId} is already enroll in course {course.Title}");
                return (false, $"Enrollment failed student with Id {enrollment.StudentId} is already enroll in course {course.Title}", 0);
            }

            var enrollmentDateisValid = await _courseRepository.CheckEnrollmentDateAsync(enrollment.CourseId, enrollment.EnrollmentDate);
            if (!enrollmentDateisValid)
            {
                _logger.LogWarning($"Service :Enrollment date for CourseId {enrollment.CourseId} must be between {course.EnrollmentStartDate} and {course.EnrollmentEndDate} .");
                return (false, $"Enrollment date for CourseId {enrollment.CourseId} must be between {course.EnrollmentStartDate} and {course.EnrollmentEndDate} .", 0);
            }

            var count = await _repository.GetEnrollmentCountByCourse(enrollment.CourseId);
            if (count >= course.Capacity)
            {
                _logger.LogWarning($"Service : Enrollment failed: Course {course.CourseId} capacity reached");
                return (false, $"Enrollment failed: Course {course.CourseId} capacity reached", 0);
            }

            var enrollCount = await _repository.GetEnrollmentCountByStudent(enrollment.StudentId);
            if (enrollCount >= maxEnrollCount)
            {
                _logger.LogError(
                      "Service : Enrollment failed: Student with ID {StudentId} has reached the maximum allowed enrollments ({MaxEnrollCount}).", enrollment.StudentId, maxEnrollCount);
                return (false, $"Student with ID {{StudentId}} has reached the maximum allowed enrollments ({{MaxEnrollCount}}).\", enrollment.StudentId, maxEnrollCount", 0);
            }

            enrollment.IsActive = true;
            enrollment.EnrollmentStatus = EnrollmentStatus.Comfirmed;
            enrollment.EnrollmentDate = DateTimeOffset.UtcNow;
            enrollment.CreatedAt = DateTimeOffset.UtcNow;
            var enrollmentId = await _repository.AddAsync(enrollment);
            _logger.LogInformation($"Enrollment created for Student id {enrollment.StudentId} for course id {enrollment.CourseId}");
            return (true, null, enrollmentId);
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            var enrollments = await _repository.GetAllAsync();

            if (!enrollments.Any() || enrollments == null)
            {
                return Enumerable.Empty<Enrollment>();
            }
            return enrollments;
        }

        public async Task<Enrollment?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(int id, Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentId)
            {
                return false;
            }
            return await _repository.UpdateAsync(id, enrollment);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }


    }
}
