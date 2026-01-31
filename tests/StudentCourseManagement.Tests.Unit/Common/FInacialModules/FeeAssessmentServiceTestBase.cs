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
    public class FeeAssessmentServiceTestBase
    {
        protected InMemoryDbContext _db;
        protected InMemoryStudentRepository _studentRepository;
        protected InMemoryCourseRepository _courseRepository;
        protected InMemoryEnrollmentRepository _enrollmentRepository;
        protected InMemoryFeeTemplateRepository _feeTemplateRepository;
        protected InMemoryFeeAssessmentRepository _assessmentRepository;
        protected InMemorryInvoiceRepository _invoiceRepository;
        protected IFeeAssessmentService _feeAssessmentService;



        [TestInitialize]
        public void Setup()
        {
            _db = new InMemoryDbContext();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<StudentProfile>();
                cfg.AddProfile<CourseProfile>();
                cfg.AddProfile<EnrollmentProfile>();
                cfg.AddProfile<FeeTemplateProfile>();
                cfg.AddProfile<FeeAssessmentProfile>();
                cfg.AddProfile<InvoiceProfile>();
                // add other profiles as needed
            });

            IMapper mapper = config.CreateMapper();

            // Mock logger
            var loggerMockFeeAss = new Mock<ILogger<FeeAssessmentService>>();

            // Initialize repositories 
            _studentRepository = new InMemoryStudentRepository(_db);
            _enrollmentRepository = new InMemoryEnrollmentRepository(mapper, _db);
            _courseRepository = new InMemoryCourseRepository(mapper, _db);


            _feeTemplateRepository = new InMemoryFeeTemplateRepository(mapper, _db);
            _assessmentRepository = new InMemoryFeeAssessmentRepository(mapper, _db);
            _invoiceRepository = new InMemorryInvoiceRepository(mapper, _db);

            // Initialize service
            _feeAssessmentService = new FeeAssessmentService(_assessmentRepository, loggerMockFeeAss.Object, _courseRepository, _enrollmentRepository, _feeTemplateRepository, _invoiceRepository);
        }
    }
}
