using Microsoft.Extensions.DependencyInjection;
using StudentCourseManagement.Business.Interfaces.Services;
using StudentCourseManagement.Business.Services;

namespace StudentCourseManagement.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #region Service Registration 
            services.AddScoped<IStudentService, StudentService>();
            #endregion


            return services;
        }
    }
}
