using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Business.Mapping;
using StudentCourseManagement.Business.Services;
using StudentCourseManagement.Data.Repositories.InMemory;

namespace StudentCourseManagement.Tests.Common
{

    public abstract class EnrollmentServiceTestBase
    {
        public IEnrollmentService _service;
        public InMemoryEnrollmentRepository _repository;
        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EnrollmentProfile>(); // add your mapping profile
            });
            IMapper mapper = config.CreateMapper();

            _repository = new InMemoryEnrollmentRepository(mapper);
            _service = new EnrollmentService(_repository);
        }
    }
}
