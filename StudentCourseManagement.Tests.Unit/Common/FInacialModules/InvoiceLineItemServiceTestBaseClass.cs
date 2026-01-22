using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Business.Mapping;
using StudentCourseManagement.Business.Mapping.FinancialModule;
using StudentCourseManagement.Data.Repositories.InMemory;
using StudentCourseManagement.Data.Repositories.InMemory.financialModule;

namespace StudentCourseManagement.Tests.Unit.Common.FInacialModules
{
    // course , feetemplate , invoice , involitemitemservce + rep 
    public abstract class InvoiceLineItemServiceTestBaseClass
    {
        protected InMemoryCourseRepository _courseRepository;
        protected InMemoryFeeTemplateRepository _feeTemplateRepository;
        protected InMemorryInvoiceRepository _invoiceRepository;
        protected InMemoryInvoiceLineItemRepository _invoiceLineItemRepository;
        protected IInvoiceLineItemService _invoiceLineItemService;
        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CourseProfile>();
                cfg.AddProfile<FeeTemplateProfile>();
                cfg.AddProfile<InvoiceLineItemProfile>();
                cfg.AddProfile<InvoiceProfile>();
            });
            var mapper = config.CreateMapper();

            _courseRepository = new InMemoryCourseRepository(mapper);
            _feeTemplateRepository = new InMemoryFeeTemplateRepository(mapper);
            _invoiceRepository = new InMemorryInvoiceRepository(mapper);
            _invoiceLineItemRepository = new InMemoryInvoiceLineItemRepository(mapper);

            var mockLogger = new Mock<ILogger<InvoiceLineItemService>>();
            _invoiceLineItemService = new InvoiceLineItemService(_invoiceLineItemRepository, mockLogger.Object, _courseRepository, _feeTemplateRepository, _invoiceRepository);

        }
    }
}
