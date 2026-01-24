using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentCourseManagement.Business.Mapping;
using StudentCourseManagement.Business.Mapping.FinancialModule;
using StudentCourseManagement.Business.Services.FinancialModule;
using StudentCourseManagement.Data.Repositories.InMemory;
using StudentCourseManagement.Data.Repositories.InMemory.financialModule;
using StudentCourseManagement.Data.Repositories.InMemory.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Common.FInacialModules
{
    //
    public class RefundServiceBaseClass
    {
        protected InMemoryPaymentRepository _paymentRepository;
        protected InMemoryCourseRepository _courseRepository;
        protected InMemorryInvoiceRepository _invoiceRepository;
        protected InMemoryFeeAssessmentRepository _feeAssessmentRepository;

        protected RefundService _refundService;
        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CourseProfile>();
                cfg.AddProfile<PaymentProfile>();
                cfg.AddProfile<InvoiceProfile>();
                cfg.AddProfile<FeeAssessmentProfile>();
            });
            var mapper = config.CreateMapper();
            _courseRepository = new InMemoryCourseRepository(mapper);
            _invoiceRepository = new InMemorryInvoiceRepository(mapper);
            _feeAssessmentRepository = new InMemoryFeeAssessmentRepository(mapper, _invoiceRepository);
            _paymentRepository = new InMemoryPaymentRepository(mapper);
            var mocklogger = new Mock<ILogger<RefundService>>();
            _refundService = new RefundService(_paymentRepository, _courseRepository, _invoiceRepository, _feeAssessmentRepository, mocklogger.Object);
        }

    }
}
