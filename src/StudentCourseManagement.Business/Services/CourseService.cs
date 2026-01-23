using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<CourseService> _logger;

        public CourseService(ICourseRepository courseRepository, ILogger<CourseService> logger)
        {
            this._courseRepository = courseRepository;
            this._logger = logger;
        }

        public async Task<(bool success, string? errorMessage, int id)> CreateAsync(Course course)
        {
            var codeExists = await _courseRepository.CodeExistsAsync(course.Code);
            if (codeExists)
            {
                _logger.LogWarning($"Repo : Failed to create course : Code {course.Code} already exists ");
                return (false, $"Code {course.Code} already exists ", 0);
            }
            var titleExists = await _courseRepository.TitleExistsAsync(course.Title);
            if (titleExists)
            {
                _logger.LogWarning($"Repo : Failed to create course : title {course.Title} already exists ");
                return (false, $"tilte {course.Title} already exists", 0);
            }
            _logger.LogInformation($"Repo : Course created suc+-cessfully with title : {course.Title}");
            //course strt date must be in advance : 
            var courseId = await _courseRepository.AddAsync(course);
            return (true, null, courseId);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _courseRepository.DeleteAsync(id);

        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            if (!courses.Any() || courses == null)
            {
                return Enumerable.Empty<Course>();
            }
            return courses;
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _courseRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return false;
            }
            return await _courseRepository.UpdateAsync(id, course);
        }
    }
}
