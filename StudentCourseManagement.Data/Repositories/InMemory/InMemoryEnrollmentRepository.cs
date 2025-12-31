using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Domain.Entities;

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
    }
}
