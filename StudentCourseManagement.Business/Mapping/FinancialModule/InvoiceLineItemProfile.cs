using AutoMapper;
using StudentCourseManagement.Domain.Entities.FinancialModule;

namespace StudentCourseManagement.Business.Mapping.FinancialModule
{
    public class InvoiceLineItemProfile : Profile
    {
        public InvoiceLineItemProfile()
        {
            CreateMap<InvoiceLineItem, InvoiceLineItem>();
        }
    }
}
