using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Business.Services;
using StudentCourseManagement.Data.Repositories.InMemory;

namespace StudentCourseManagement.Tests.Common
{
    [TestClass]
    public abstract class StudentServiceTestBase
    {
        protected IStudentService _studentService;

        [TestInitialize]
        public void Setup()
        {
            var repo = new InMemoryStudentRepository();
            _studentService = new StudentService(repo);
        }
    }
}
