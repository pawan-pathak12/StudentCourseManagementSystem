using AutoMapper;
using StudentCourseManagement.Application.DTOs.DTOs.FInancialModule.FeeAssessments;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Mapping.FinancialModule
{
    public class FeeAssessmentProfile : Profile
    {
        public FeeAssessmentProfile()
        {
            CreateMap<FeeAssessment, FeeAssessment>();
            CreateMap<FeeAssessment, CreateFeeAssessmentDto>().ReverseMap();
            CreateMap<FeeAssessment, UpdateFeeAssessmentDto>().ReverseMap();


        }
    }
}
