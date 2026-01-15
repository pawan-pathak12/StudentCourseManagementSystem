using AutoMapper;
using StudentCourseManagement.API.DTOs.FInancialModule.InvoiceLineItems;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Mapping.FinancialModule
{
    public class InvoiceLineItemProfile : Profile
    {
        public InvoiceLineItemProfile()
        {
            CreateMap<InvoiceLineItem, InvoiceLineItem>();
            CreateMap<InvoiceLineItem, CreateInvoiceLineItemDto>().ReverseMap();
            CreateMap<InvoiceLineItem, UpdateInvoiceLineItemDto>().ReverseMap();
        }
    }
}
