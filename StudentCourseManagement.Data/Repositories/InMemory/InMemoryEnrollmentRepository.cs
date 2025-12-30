using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Data.Repositories.InMemory
{
    public class InMemoryEnrollmentRepository : IEnrollmentRepository
    {
        private readonly List<Student> _students;
        private readonly List<Course> _course;
        public readonly List<Enrollment> _enrollments;
        private readonly IMapper _mapper;

        public InMemoryEnrollmentRepository(IMapper mapper)
        {
            _enrollments = new List<Enrollment>();
            _students = new List<Student>();
            _course = new List<Course>();
            this._mapper = mapper;
        }
        public Task<int> AddAsync(Enrollment enrollment)
        {
            if (_students.Find(x => x.StudentId == enrollment.StudentId))
            {

            }
            _enrollments.Add(enrollment);
            enrollment.EnrollmentId++;
            return Task.FromResult(enrollment.EnrollmentId);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var enrollment = _enrollments
                 .FirstOrDefault(e => e.EnrollmentId == id && e.IsActive);

            if (enrollment == null)
            {
                return Task.FromResult(false);
            }

            // Soft delete
            enrollment.IsActive = false;

            return Task.FromResult(true);
        }

        public Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            var enrollments = _enrollments.AsEnumerable();
            if (!enrollments.Any())
            {
                return Task.FromResult(Enumerable.Empty<Enrollment>());
            }
            return Task.FromResult(enrollments);
        }

        public Task<Enrollment?> GetByIdAsync(int id)
        {
            var enrollment = _enrollments
                .FirstOrDefault(e => e.EnrollmentId == id && e.IsActive);
            return Task.FromResult(enrollment);
        }

        public Task<bool> UpdateAsync(int id, Enrollment enrollment)
        {
            var existing = _enrollments
                  .FirstOrDefault(e => e.EnrollmentId == id && e.IsActive);

            if (existing == null)
            {
                return Task.FromResult(false);
            }

            _mapper.Map(enrollment, existing);

            return Task.FromResult(true);
        }
    }
}
