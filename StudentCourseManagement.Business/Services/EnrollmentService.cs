using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Domain.Entities;

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

        public async Task<bool> CreateAsync(Enrollment enrollment)
        {
            var student = await _studentRepository.GetByIdAsync(enrollment.StudentId);
            if (student == null)
            {
                _logger.LogWarning($"Service : Enrollment Failed : Student with Id {enrollment.StudentId} not found or is inactive");
                return false;
            }

            var course = await _courseRepository.GetByIdAsync(enrollment.CourseId);
            if (course == null)
            {
                _logger.LogWarning($"Service : Enrollment Failed : Course with Id {enrollment.CourseId} not found or is inactive");
                return false;
            }

            var enrollmentExists = await _repository.ExistsAsync(enrollment.StudentId, enrollment.CourseId);

            if (enrollmentExists)
            {
                _logger.LogWarning($"Service : Enrollment failed student with Id {enrollment.StudentId} is already enroll in course {course.Title}");
                return false;
            }

            var count = await _repository.GetEnrollmentCountByCourse(enrollment.CourseId);
            if (count >= course.Capacity)
            {
                _logger.LogWarning($"Service : Enrollment failed: Course {course.CourseId} capacity reached");
                return false;
            }

            var enrollCount = await _repository.GetEnrollmentCountByStudent(enrollment.StudentId);
            if (enrollCount >= maxEnrollCount)
            {
                _logger.LogError(
                      "Service : Enrollment failed: Student with ID {StudentId} has reached the maximum allowed enrollments ({MaxEnrollCount}).", enrollment.StudentId, maxEnrollCount);
                return false;
            }


            await _repository.AddAsync(enrollment);
            _logger.LogInformation($"Enrollment created for Student id {enrollment.StudentId} for course id {enrollment.CourseId}");
            return true;
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
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
