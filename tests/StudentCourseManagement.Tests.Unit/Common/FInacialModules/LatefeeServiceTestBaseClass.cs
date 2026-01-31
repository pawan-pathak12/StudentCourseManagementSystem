using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Business.Mapping.FinancialModule;
using StudentCourseManagement.Business.Services.FinancialModule;
using StudentCourseManagement.Data.Repositories.InMemory;
using StudentCourseManagement.Data.Repositories.InMemory.financialModule;
using StudentCourseManagement.Data.Repositories.InMemory.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Common.FInacialModules
{
    //invoice + feeassessment , lineitem repo + latefeeservice 
    public abstract class LatefeeServiceTestBaseClass
    {
        protected InMemoryDbContext _db;
        protected ILateFeeService _lateFeeService;
        protected InMemoryFeeAssessmentRepository _feeAssessmentRepository;
        protected InMemoryInvoiceLineItemRepository _lineItemRepository;
        protected InMemorryInvoiceRepository _invoiceRepository;

        [TestInitialize]
        public void Setup()
        {
            _db = new InMemoryDbContext();
            var cfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<FeeAssessmentProfile>();
                cfg.AddProfile<InvoiceProfile>();
                cfg.AddProfile<InvoiceLineItemProfile>();
            });
            var mapper = cfg.CreateMapper();

            var mockLogger = new Mock<ILogger<LateFeeService>>();

            _feeAssessmentRepository = new InMemoryFeeAssessmentRepository(mapper, _db);
            _invoiceRepository = new InMemorryInvoiceRepository(mapper, _db);
            _lineItemRepository = new InMemoryInvoiceLineItemRepository(mapper, _db);
            _lateFeeService = new LateFeeService(_invoiceRepository, mockLogger.Object, _feeAssessmentRepository, _lineItemRepository);
        }
    }
}
