using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Business.Mapping;
using StudentCourseManagement.Business.Services;
using StudentCourseManagement.Data.Repositories.InMemory;
using StudentCourseManagement.Data.Repositories.InMemory.AcademicModule;

namespace StudentCourseManagement.Tests.Unit.Common
{

    public abstract class EnrollmentServiceTestBase
    {
        protected InMemoryDbContext _db;
        public IEnrollmentService _service;
        public InMemoryEnrollmentRepository _repository;
        public InMemoryStudentRepository _studentRepository;
        public InMemoryCourseRepository _courseRepository;
        [TestInitialize]
        public void Setup()
        {
            _db = new InMemoryDbContext();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EnrollmentProfile>();
            });
            IMapper mapper = config.CreateMapper();

            var logger = new Mock<ILogger<EnrollmentService>>();

            _studentRepository = new InMemoryStudentRepository(_db);
            _repository = new InMemoryEnrollmentRepository(mapper, _db);
            _courseRepository = new InMemoryCourseRepository(mapper, _db);


            _service = new EnrollmentService(_repository, _studentRepository, _courseRepository, logger.Object);
        }
    }
}
