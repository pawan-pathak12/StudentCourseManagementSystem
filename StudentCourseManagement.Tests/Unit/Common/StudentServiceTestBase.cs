using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Business.Services;
using StudentCourseManagement.Data.Repositories.InMemory;

namespace StudentCourseManagement.Tests.Unit.Common
{
    [TestClass]
    public abstract class StudentServiceTestBase
    {
        protected IStudentService _studentService;
        protected InMemoryStudentRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _repository = new InMemoryStudentRepository();
            _studentService = new StudentService(_repository);

        }
    }
}
