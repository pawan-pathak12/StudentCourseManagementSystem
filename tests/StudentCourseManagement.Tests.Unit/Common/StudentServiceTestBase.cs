using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Business.Services;
using StudentCourseManagement.Data.Repositories.InMemory;
using StudentCourseManagement.Data.Repositories.InMemory.AcademicModule;

namespace StudentCourseManagement.Tests.Unit.Common
{
    [TestClass]
    public abstract class StudentServiceTestBase
    {
        protected InMemoryDbContext _db;
        protected IStudentService _studentService;
        protected InMemoryStudentRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _db = new InMemoryDbContext();
            _repository = new InMemoryStudentRepository(_db);
            _studentService = new StudentService(_repository);
        }
    }
}
