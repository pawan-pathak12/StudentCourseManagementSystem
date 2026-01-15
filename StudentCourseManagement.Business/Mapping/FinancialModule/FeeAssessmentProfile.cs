using AutoMapper;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Mapping.FinancialModule
{
    public class FeeAssessmentProfile : Profile
    {
        public FeeAssessmentProfile()
        {
            CreateMap<FeeAssessment, FeeAssessment>();

        }
    }
}
