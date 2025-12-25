using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Data.Repositories.InMemory
{
    public class InMemoryCourseRepository : ICourseRepository
    {
        public readonly List<Course> _courses;
        private readonly IMapper _mapper;
        public InMemoryCourseRepository(IMapper mapper)
        {
            _courses = new List<Course>();
            this._mapper = mapper;
        }

        public Task<int> CreateAsync(Course course)
        {
            _courses.Add(course);
            return Task.FromResult(course.CourseId);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var exisitngCourse = _courses.Find(x => x.CourseId == id);
            if (exisitngCourse == null)
            {
                return Task.FromResult(false);
            }
            var isdeleted = _courses.Remove(exisitngCourse);
            return Task.FromResult(isdeleted);
        }

        public Task<IEnumerable<Course>> GetAllAsync()
        {
            var courses = _courses.AsEnumerable();
            return Task.FromResult(courses);
        }

        public Task<Course> GetByIdAsync(int id)
        {
            var exisitngCourse = _courses.Find(x => x.CourseId == id);

            return Task.FromResult(exisitngCourse);
        }

        public Task<bool> UpdateAsync(int id, Course course)
        {
            var exisitngCourse = _courses.Find(x => x.CourseId == id);
            if (exisitngCourse == null)
            {
                return Task.FromResult(true);
            }
            _mapper.Map(course, exisitngCourse);

            return Task.FromResult(true);
        }
    }
}
