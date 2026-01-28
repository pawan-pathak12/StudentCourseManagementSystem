using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Business.Mapping;
using StudentCourseManagement.Business.Services;
using StudentCourseManagement.Data.Repositories.InMemory;

namespace StudentCourseManagement.Tests.Unit.Common
{

    public abstract class EnrollmentServiceTestBase
    {
        public IEnrollmentService _service;
        public InMemoryEnrollmentRepository _repository;
        public InMemoryStudentRepository _studentRepository;
        public InMemoryCourseRepository _courseRepository;
        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EnrollmentProfile>(); // add your mapping profile
            });
            IMapper mapper = config.CreateMapper();

            var logger = new Mock<ILogger<EnrollmentService>>();

            _studentRepository = new InMemoryStudentRepository();
            _repository = new InMemoryEnrollmentRepository(mapper);
            _courseRepository = new InMemoryCourseRepository(mapper, _repository);


            _service = new EnrollmentService(_repository, _studentRepository, _courseRepository, logger.Object);
        }
    }
}
