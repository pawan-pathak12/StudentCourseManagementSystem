using AutoMapper;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Mapping.FinancialModule
{
    public class FeeTemplateProfile : Profile
    {
        public FeeTemplateProfile()
        {
            CreateMap<FeeTemplate, FeeTemplate>();
        }
    }
}
