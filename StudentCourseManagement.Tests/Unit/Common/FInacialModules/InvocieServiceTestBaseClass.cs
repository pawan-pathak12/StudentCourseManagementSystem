using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Business.Mapping;
using StudentCourseManagement.Business.Mapping.FinancialModule;
using StudentCourseManagement.Business.Services.FinancialModule;
using StudentCourseManagement.Data.Repositories.InMemory;
using StudentCourseManagement.Data.Repositories.InMemory.financialModule;
using StudentCourseManagement.Data.Repositories.InMemory.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Common.FInacialModules
{
    public abstract class InvocieServiceTestBaseClass
    {
        protected IInvoiceService _invoiceService;

        protected InMemoryStudentRepository _studentRepository;
        protected InMemoryCourseRepository _courseRepository;
        protected InMemoryFeeAssessmentRepository _feeAssessmentRepository;
        protected InMemorryInvoiceRepository _invoiceRepository;
        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<StudentProfile>();
                cfg.AddProfile<CourseProfile>();
                cfg.AddProfile<FeeAssessmentProfile>();
            });
            IMapper mapper = config.CreateMapper();

            _studentRepository = new InMemoryStudentRepository();
            _courseRepository = new InMemoryCourseRepository(mapper);
            _feeAssessmentRepository = new InMemoryFeeAssessmentRepository(mapper);
            _invoiceRepository = new InMemorryInvoiceRepository(mapper);

            var mockLogger = new Mock<ILogger<InvoiceService>>();
            _invoiceService = new InvoiceService(_invoiceRepository, mockLogger.Object, _studentRepository, _courseRepository, _feeAssessmentRepository);
        }

    }
}
