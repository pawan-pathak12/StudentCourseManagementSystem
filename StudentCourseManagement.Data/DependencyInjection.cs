using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Data.Database;
using StudentCourseManagement.Data.Repositories.Dapper;

namespace StudentCourseManagement.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<StudentSysDbContext>();

            #region Repository Registration 
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            #endregion

            return services;
        }
    }
}
