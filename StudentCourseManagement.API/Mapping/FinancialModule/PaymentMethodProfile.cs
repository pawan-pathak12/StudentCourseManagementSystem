using AutoMapper;
using StudentCourseManagement.Application.DTOs.DTOs.FInancialModule.PaymentMethods;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Mapping.FinancialModule
{
    public class PaymentMethodProfile : Profile
    {
        public PaymentMethodProfile()
        {
            CreateMap<PaymentMethod, PaymentMethod>();
            CreateMap<PaymentMethod, CreatePaymentMethodDto>().ReverseMap();
            CreateMap<PaymentMethod, UpdatePaymentMethodDto>().ReverseMap();
        }
    }
}
