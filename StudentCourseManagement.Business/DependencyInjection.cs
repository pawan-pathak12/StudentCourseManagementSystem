using Microsoft.Extensions.DependencyInjection;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Business.Interfaces.Services.FinancialModule;
using StudentCourseManagement.Business.Services;
using StudentCourseManagement.Business.Services.FinancialModule;

namespace StudentCourseManagement.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
        {

            #region Service Registration 
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();

            services.AddScoped<IFeeAssessmentService, FeeAssessmentService>();
            services.AddScoped<IFeeTemplateService, FeeTemplateService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IInvoiceLineItemService, InvoiceLineItemService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            #endregion

            return services;
        }
    }
}
