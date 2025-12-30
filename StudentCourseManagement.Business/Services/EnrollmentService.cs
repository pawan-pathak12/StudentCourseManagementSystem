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

        public EnrollmentService(IEnrollmentRepository repository, IStudentRepository studentRepository, ICourseRepository courseRepository, ILogger<EnrollmentService> logger)
        {
            _repository = repository;
            this._studentRepository = studentRepository;
            this._courseRepository = courseRepository;
            this._logger = logger;
        }

        public async Task<int> CreateAsync(Enrollment enrollment)
        {
            var student = await _studentRepository.GetByIdAsync(enrollment.StudentId);
            if (student == null)
            {
                _logger.LogWarning($"Service : Enrollment Failed : Student with Id {enrollment.StudentId} not found or is inactive");
                return 0;
            }
            var course = await _courseRepository.GetByIdAsync(enrollment.CourseId);
            if (course == null)
            {
                _logger.LogWarning($"Service : Enrollment Failed : Course with Id {enrollment.CourseId} not found or is inactive");
                return 0;  // later it will be fixed by changing method return type to bool 
            }

            var newId = await _repository.AddAsync(enrollment);
            _logger.LogInformation($"Enrollment created for Student id {enrollment.StudentId} for course id {enrollment.CourseId}");
            return newId;
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
