using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Domain.Entities;
using StudentCourseManagement.Domain.Enums;

namespace StudentCourseManagement.Data.Repositories.InMemory
{
    public class InMemoryEnrollmentRepository : IEnrollmentRepository
    {
        public readonly List<Enrollment> _enrollments;
        private readonly IMapper _mapper;

        public InMemoryEnrollmentRepository(IMapper mapper)
        {
            _enrollments = new List<Enrollment>();
            this._mapper = mapper;
        }

        #region CURD Operations 
        public Task<int> AddAsync(Enrollment enrollment)
        {
            var newId = _enrollments.Count + 1;
            enrollment.EnrollmentId = newId;

            _enrollments.Add(enrollment);
            return Task.FromResult(newId);
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

        #endregion


        #region Business Logic Required Methods

        public Task<int> GetEnrollmentCountByCourse(int courseId)
        {
            var count = _enrollments
          .Count(e => e.CourseId == courseId);

            return Task.FromResult(count);
        }

        public Task<bool> ExistsAsync(int studentId, int courseId)
        {
            var isDuplicate = _enrollments.Any(x => x.StudentId == studentId && x.CourseId == courseId);
            return Task.FromResult(isDuplicate);
        }

        public Task<int> GetEnrollmentCountByStudent(int studentId)
        {
            var count = (from a in _enrollments where a.StudentId == studentId && a.EnrollmentStatus == EnrollmentStatus.Comfirmed select a).Count();
            return Task.FromResult(count);

        }


        #endregion
    }
}
