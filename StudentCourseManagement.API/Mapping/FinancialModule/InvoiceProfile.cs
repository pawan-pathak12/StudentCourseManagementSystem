using AutoMapper;
using StudentCourseManagement.Application.DTOs.DTOs.FInancialModule.Invoices;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Mapping.FinancialModule
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<Invoice, Invoice>();
            CreateMap<Invoice, CreateInvoiceDto>().ReverseMap();
            CreateMap<Invoice, UpdateInvoiceDto>().ReverseMap();
        }
    }
}
