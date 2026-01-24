using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Business.Mapping;
using StudentCourseManagement.Business.Mapping.FinancialModule;
using StudentCourseManagement.Data.Repositories.InMemory;
using StudentCourseManagement.Data.Repositories.InMemory.financialModule;
using StudentCourseManagement.Data.Repositories.InMemory.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Common.FInacialModules
{
    // student , invocice , paymentmethod , paymentrepo + service
    public abstract class PaymentServiceTestBase
    {
        protected InMemoryStudentRepository _studentRepository;
        protected InMemoryCourseRepository _courseRepository;
        protected InMemoryEnrollmentRepository _enrollmentRepository;
        protected InMemorryInvoiceRepository _invoiceRepository;
        protected InMemoryPaymentMethodRepository _paymentMethodRepository;
        protected InMemoryPaymentRepository _paymentRepository;
        protected InMemoryFeeAssessmentRepository _feeAssessmentRepository;
        protected InMemoryFeeTemplateRepository _feeTemplateRepository;
        protected IPaymentService _paymentService;
        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<StudentProfile>();
                cfg.AddProfile<CourseProfile>();
                cfg.AddProfile<EnrollmentProfile>();
                cfg.AddProfile<InvoiceProfile>();
                cfg.AddProfile<PaymentProfile>();
                cfg.AddProfile<PaymentMethodProfile>();
                cfg.AddProfile<FeeAssessmentProfile>();
                cfg.AddProfile<FeeTemplateProfile>();
            });

            var mapper = config.CreateMapper();
            var mockLoggerPayment = new Mock<ILogger<PaymentService>>();

            _studentRepository = new InMemoryStudentRepository();
            _courseRepository = new InMemoryCourseRepository(mapper);
            _enrollmentRepository = new InMemoryEnrollmentRepository(mapper);
            _invoiceRepository = new InMemorryInvoiceRepository(mapper);
            _paymentMethodRepository = new InMemoryPaymentMethodRepository(mapper);
            _paymentRepository = new InMemoryPaymentRepository(mapper);
            _feeAssessmentRepository = new InMemoryFeeAssessmentRepository(mapper, _invoiceRepository);
            _feeTemplateRepository = new InMemoryFeeTemplateRepository(mapper);

            _paymentService = new PaymentService(_paymentRepository, mockLoggerPayment.Object, _studentRepository, _paymentMethodRepository, _invoiceRepository, _feeAssessmentRepository);

        }
    }
}
