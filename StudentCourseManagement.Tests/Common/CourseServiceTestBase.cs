using AutoMapper;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Business.Mapping;
using StudentCourseManagement.Business.Services;
using StudentCourseManagement.Data.Repositories.InMemory;

namespace StudentCourseManagement.Tests.Common
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

            _courseRepository = new InMemoryCourseRepository(mapper);
            _courseService = new CourseService(_courseRepository);
        }
    }
}
