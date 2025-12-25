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

        public async Task<int> Create(Course course)
        {
            var created = await _courseRepository.CreateAsync(course);
            return created;
        }

        public async Task<bool> Delete(int id)
        {
            return await _courseRepository.DeleteAsync(id);

        }

        public async Task<IEnumerable<Course>> GetAll()
        {
            return await _courseRepository.GetAllAsync();
        }

        public async Task<Course?> GetById(int id)
        {
            return await _courseRepository.GetByIdAsync(id);
        }

        public async Task<bool> Update(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return false;
            }
            return await _courseRepository.UpdateAsync(id, course);
        }
    }
}
