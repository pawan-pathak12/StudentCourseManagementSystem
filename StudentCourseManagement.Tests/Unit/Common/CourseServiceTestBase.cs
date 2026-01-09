using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Business.Mapping;
using StudentCourseManagement.Business.Services;
using StudentCourseManagement.Data.Repositories.InMemory;

namespace StudentCourseManagement.Tests.Unit.Common
{
    public class CourseServiceTestBase
    {
        protected InMemoryCourseRepository _courseRepository;
        protected ICourseService _courseService;


        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CourseProfile>(); // add your mapping profile
            });
            IMapper mapper = config.CreateMapper();
            var loggerMock = new Mock<ILogger<CourseService>>();

            _courseRepository = new InMemoryCourseRepository(mapper);
            _courseService = new CourseService(_courseRepository, loggerMock.Object);
        }
    }
}
