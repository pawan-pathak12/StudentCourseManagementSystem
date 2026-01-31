using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Data.Repositories.InMemory.AcademicModule
{
    public class InMemoryCourseRepository : ICourseRepository
    {
        public readonly List<Course> _courses;
        public readonly List<Enrollment> _enrollments;
        private readonly InMemoryDbContext _db;

        private readonly IMapper _mapper;
        public InMemoryCourseRepository(IMapper mapper, InMemoryDbContext db)
        {
            this._db = db;
            _courses = _db.Courses;
            _enrollments = _db.Enrollments;
            this._mapper = mapper;

        }

        #region CURD Operations 
        public Task<int> AddAsync(Course course)
        {
            var newId = _courses.Count + 1;
            course.CourseId = newId;
            _courses.Add(course);
            return Task.FromResult(newId);

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
            var exisitngCourse = _courses.Find(x => x.CourseId == id && x.IsActive == true);

            return Task.FromResult(exisitngCourse);
        }



        public Task<bool> UpdateAsync(int id, Course course)
        {
            var exisitngCourse = _courses.Find(x => x.CourseId == id);
            if (exisitngCourse == null)
            {
                return Task.FromResult(false);
            }
            _mapper.Map(course, exisitngCourse);

            return Task.FromResult(true);
        }
        #endregion

        #region Course Validation

        public Task<bool> CodeExistsAsync(string code)
        {
            var course = _courses.Find(x => x.Code == code);
            if (course == null)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public Task<bool> TitleExistsAsync(string title)
        {
            var course = _courses.Find(x => x.Title == title);
            if (course == null)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
        #endregion

        public Task<bool> CheckEnrollmentDateAsync(int courseId, DateTimeOffset enrollmentDate)
        {
            var course = from a in _courses where a.CourseId == courseId select a;

            var isValid = (from a in course
                           where enrollmentDate >= a.EnrollmentStartDate
                              && enrollmentDate < a.EnrollmentEndDate
                           select a).Any();

            return Task.FromResult(isValid);
        }

        public Task<DateTimeOffset> GetStartDateByEnrollmentIdAsync(int enrollmentId)
        {
            var enrollment = _enrollments.Find(x => x.EnrollmentId == enrollmentId);
            if (enrollment == null)
            {

            }
            var course = _courses.Find(x => x.CourseId == enrollment.CourseId);
            return Task.FromResult(course.StartDate);
        }
    }
}
