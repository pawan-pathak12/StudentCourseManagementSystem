using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            this._courseRepository = courseRepository;
        }

        public async Task<int> CreateAsync(Course course)
        {
            var created = await _courseRepository.AddAsync(course);
            return created;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _courseRepository.DeleteAsync(id);

        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _courseRepository.GetAllAsync();
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
