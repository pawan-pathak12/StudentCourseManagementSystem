using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Business.Mapping;
using StudentCourseManagement.Business.Mapping.FinancialModule;
using StudentCourseManagement.Data.Repositories.InMemory;
using StudentCourseManagement.Data.Repositories.InMemory.AcademicModule;
using StudentCourseManagement.Data.Repositories.InMemory.financialModule;
using StudentCourseManagement.Data.Repositories.InMemory.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Common.FInacialModules
{
    // course , feetemplate , invoice , involitemitemservce + rep 
    public abstract class InvoiceLineItemServiceTestBaseClass
    {
        protected InMemoryDbContext _db;
        protected InMemoryStudentRepository _studentRepository;
        protected InMemoryCourseRepository _courseRepository;
        protected InMemoryEnrollmentRepository _enrollmentRepository;
        protected InMemoryFeeTemplateRepository _feeTemplateRepository;
        protected InMemorryInvoiceRepository _invoiceRepository;
        protected InMemoryInvoiceLineItemRepository _invoiceLineItemRepository;
        protected IInvoiceLineItemService _invoiceLineItemService;
        protected InMemoryFeeAssessmentRepository _feeAssessmentRepository;

        [TestInitialize]
        public void Setup()
        {
            _db = new InMemoryDbContext();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CourseProfile>();
                cfg.AddProfile<EnrollmentProfile>();
                cfg.AddProfile<InvoiceProfile>();
                cfg.AddProfile<FeeTemplateProfile>();
                cfg.AddProfile<InvoiceLineItemProfile>();
                cfg.AddProfile<InvoiceProfile>();
            });

            var mapper = config.CreateMapper();
            _studentRepository = new InMemoryStudentRepository(_db);
            _enrollmentRepository = new InMemoryEnrollmentRepository(mapper, _db);

            _courseRepository = new InMemoryCourseRepository(mapper, _db);
            _feeTemplateRepository = new InMemoryFeeTemplateRepository(mapper, _db);

            _feeAssessmentRepository = new InMemoryFeeAssessmentRepository(mapper, _db);

            _invoiceRepository = new InMemorryInvoiceRepository(mapper, _db);
            _invoiceLineItemRepository = new InMemoryInvoiceLineItemRepository(mapper, _db);

            var mockLogger = new Mock<ILogger<InvoiceLineItemService>>();
            _invoiceLineItemService = new InvoiceLineItemService(_invoiceLineItemRepository, mockLogger.Object, _courseRepository, _feeTemplateRepository, _invoiceRepository);

        }
    }
}
