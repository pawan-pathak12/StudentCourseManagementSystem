using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Business.Mapping;
using StudentCourseManagement.Business.Mapping.FinancialModule;
using StudentCourseManagement.Business.Services.FinancialModule;
using StudentCourseManagement.Data.Repositories.InMemory;
using StudentCourseManagement.Data.Repositories.InMemory.AcademicModule;
using StudentCourseManagement.Data.Repositories.InMemory.financialModule;
using StudentCourseManagement.Data.Repositories.InMemory.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Common.FInacialModules
{
    public abstract class InvocieServiceTestBaseClass
    {
        protected IInvoiceService _invoiceService;
        protected InMemoryDbContext _db;
        protected InMemoryStudentRepository _studentRepository;
        protected InMemoryCourseRepository _courseRepository;
        protected InMemoryEnrollmentRepository _enrollmentRepository;
        protected InMemoryFeeAssessmentRepository _feeAssessmentRepository;
        protected InMemorryInvoiceRepository _invoiceRepository;
        [TestInitialize]
        public void Setup()
        {
            _db = new InMemoryDbContext();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<StudentProfile>();
                cfg.AddProfile<CourseProfile>();
                cfg.AddProfile<EnrollmentProfile>();
                cfg.AddProfile<FeeAssessmentProfile>();
                cfg.AddProfile<InvoiceProfile>();
            });
            IMapper mapper = config.CreateMapper();

            _studentRepository = new InMemoryStudentRepository(_db);
            _enrollmentRepository = new InMemoryEnrollmentRepository(mapper, _db);
            _courseRepository = new InMemoryCourseRepository(mapper, _db);
            _feeAssessmentRepository = new InMemoryFeeAssessmentRepository(mapper, _db);
            _invoiceRepository = new InMemorryInvoiceRepository(mapper, _db);

            var mockLogger = new Mock<ILogger<InvoiceService>>();
            _invoiceService = new InvoiceService(_invoiceRepository, mockLogger.Object, _studentRepository, _courseRepository, _feeAssessmentRepository);
        }

    }
}
