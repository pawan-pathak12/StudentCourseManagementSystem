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

        public async Task<int> CreateEnrollmentAsync(Enrollment enrollment)
        {
            var newId = await _repository.Create(enrollment);
            return newId;
        }

        public async Task<IEnumerable<Enrollment>> GetAllEnrollmentsAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<Enrollment?> GetEnrollmentByIdAsync(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<bool> UpdateEnrollmentAsync(int id, Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentId)
            {
                return false;
            }
            return await _repository.Update(id, enrollment);
        }

        public async Task<bool> DeleteEnrollmentAsync(int id)
        {
            return await _repository.Delete(id);
        }


    }
}
