using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Business.Interfaces.Repositories.FinancialModule;
using StudentCourseManagement.Business.Interfaces.Repositories.Identities;
using StudentCourseManagement.Data.Database;
using StudentCourseManagement.Data.Repositories.Dapper;
using StudentCourseManagement.Data.Repositories.Dapper.FinancialModule;
using StudentCourseManagement.Data.Repositories.Dapper.Identities;

namespace StudentCourseManagement.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddTransient<StudentSysDbContext>();

            #region Repository Registration 

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();

            services.AddScoped<IFeeAssessmentRepository, FeeAssessmentRepository>();
            services.AddScoped<IFeeTemplateRepository, FeeTemplateRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IInvoiceLineItemRepository, InvoiceLineItemRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
            #endregion

            return services;
        }
    }
}
