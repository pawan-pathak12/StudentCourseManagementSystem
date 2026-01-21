using AutoMapper;
using StudentCourseManagement.Application.DTOs.FInancialModule.feeTemplates;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Mapping.FinancialModule
{
    public class FeeTemplateProfile : Profile
    {
        public FeeTemplateProfile()
        {
            CreateMap<FeeTemplate, FeeTemplate>();
            CreateMap<FeeTemplate, CreateFeeTemplateDto>().ReverseMap();
            CreateMap<FeeTemplate, UpdateFeeTemplateDto>().ReverseMap();
        }
    }
}
