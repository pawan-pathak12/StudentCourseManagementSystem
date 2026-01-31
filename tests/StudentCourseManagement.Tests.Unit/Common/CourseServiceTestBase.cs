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
    public class CourseServiceTestBase
    {
        protected InMemoryDbContext _db;
        protected InMemoryCourseRepository _courseRepository;
        protected ICourseService _courseService;


        [TestInitialize]
        public void Setup()
        {
            _db = new InMemoryDbContext();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CourseProfile>(); // add your mapping profile
            });
            IMapper mapper = config.CreateMapper();

            var loggerMock = new Mock<ILogger<CourseService>>();

            _courseRepository = new InMemoryCourseRepository(mapper, _db);
            _courseService = new CourseService(_courseRepository, loggerMock.Object);
        }
    }
}
