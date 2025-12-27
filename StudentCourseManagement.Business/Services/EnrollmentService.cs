using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _repository;

        public EnrollmentService(IEnrollmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateAsync(Enrollment enrollment)
        {
            var newId = await _repository.AddAsync(enrollment);
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
