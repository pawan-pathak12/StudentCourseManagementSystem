using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Business.Mapping;
using StudentCourseManagement.Business.Mapping.FinancialModule;
using StudentCourseManagement.Business.Services.FinancialModule;
using StudentCourseManagement.Data.Repositories.InMemory;
using StudentCourseManagement.Data.Repositories.InMemory.AcademicModule;
using StudentCourseManagement.Data.Repositories.InMemory.financialModule;
using StudentCourseManagement.Data.Repositories.InMemory.FinancialModule;

namespace StudentCourseManagement.Tests.Unit.Common.FInacialModules
{
    //
    public class RefundServiceBaseClass
    {
        protected InMemoryDbContext _db;
        protected InMemoryStudentRepository _studentRepository;
        protected InMemoryEnrollmentRepository _enrollmentRepository;
        protected InMemoryFeeTemplateRepository _feeTemplateRepository;
        protected InMemoryPaymentRepository _paymentRepository;
        protected InMemoryCourseRepository _courseRepository;
        protected InMemorryInvoiceRepository _invoiceRepository;
        protected InMemoryFeeAssessmentRepository _feeAssessmentRepository;
        protected InMemoryPaymentMethodRepository _paymentMethodRepository;

        protected RefundService _refundService;
        [TestInitialize]
        public void Setup()
        {
            _db = new InMemoryDbContext();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<StudentProfile>();
                cfg.AddProfile<CourseProfile>();
                cfg.AddProfile<EnrollmentProfile>();
                cfg.AddProfile<PaymentProfile>();
                cfg.AddProfile<FeeTemplateProfile>();
                cfg.AddProfile<InvoiceProfile>();
                cfg.AddProfile<FeeAssessmentProfile>();
                cfg.AddProfile<PaymentMethodProfile>();
            });
            var mapper = config.CreateMapper();
            _studentRepository = new InMemoryStudentRepository(_db);
            _enrollmentRepository = new InMemoryEnrollmentRepository(mapper, _db);
            _courseRepository = new InMemoryCourseRepository(mapper, _db);

            _feeTemplateRepository = new InMemoryFeeTemplateRepository(mapper, _db);
            _feeAssessmentRepository = new InMemoryFeeAssessmentRepository(mapper, _db);
            _paymentRepository = new InMemoryPaymentRepository(mapper, _db);

            _invoiceRepository = new InMemorryInvoiceRepository(mapper, _db);

            _paymentMethodRepository = new InMemoryPaymentMethodRepository(mapper, _db);
            var mocklogger = new Mock<ILogger<RefundService>>();

            _refundService = new RefundService(_paymentRepository, _courseRepository, _invoiceRepository, _feeAssessmentRepository, mocklogger.Object);
        }

    }
}
