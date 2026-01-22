using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Business.Mapping;
using StudentCourseManagement.Business.Mapping.FinancialModule;
using StudentCourseManagement.Business.Services.FinancialModule;
using StudentCourseManagement.Data.Repositories.InMemory;
using StudentCourseManagement.Data.Repositories.InMemory.financialModule;

namespace StudentCourseManagement.Tests.Unit.Common.FInacialModules
{
    public class FeeTemplateServiceBase
    {
        protected IFeeTemplateService _feeTemplateService;
        protected InMemoryCourseRepository _courseRepository;
        protected InMemoryFeeTemplateRepository _feeTemplateRepository;
        [TestInitialize]
        public void Setup()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<FeeTemplateProfile>();
                cfg.AddProfile<CourseProfile>();
            });
            var mapper = config.CreateMapper();
            var mockLogger = new Mock<ILogger<FeeTemplateService>>();

            _courseRepository = new InMemoryCourseRepository(mapper);
            _feeTemplateRepository = new InMemoryFeeTemplateRepository(mapper);
            _feeTemplateService = new FeeTemplateService(_feeTemplateRepository, mockLogger.Object, _courseRepository);
        }
    }
}
