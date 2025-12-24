using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Business.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository repository)
        {

            _repository = repository;
        }
        public StudentService(IMapper mapper)
        {
            this._mapper = mapper;

        }
        public async Task Create(Student student)
        {
            await _repository.Add(student);
        }



        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Student>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Student?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(int id, Student dto)
        {
            throw new NotImplementedException();
        }
    }
}
