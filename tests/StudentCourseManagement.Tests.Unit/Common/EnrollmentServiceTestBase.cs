using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
        public ILogger<EnrollmentService> logger;

        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EnrollmentProfile>(); // add your mapping profile
            });
            IMapper mapper = config.CreateMapper();
            logger = NullLogger<EnrollmentService>.Instance;
            _studentRepository = new InMemoryStudentRepository();
            _courseRepository = new InMemoryCourseRepository(mapper);
            _repository = new InMemoryEnrollmentRepository(mapper);

            _service = new EnrollmentService(_repository, _studentRepository, _courseRepository, logger);
        }
    }
}
