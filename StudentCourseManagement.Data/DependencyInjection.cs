using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StudentCourseManagement.Business.Interfaces.Repositories;
using StudentCourseManagement.Data.Database;
using StudentCourseManagement.Data.Repositories.Dapper;

namespace StudentCourseManagement.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
        {
            #region Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .MinimumLevel.Information() // default level for your logs
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning) // suppress ASP.NET Core info logs
                .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)    // suppress System info logs
                .WriteTo.Console()
                        .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Register Serilog as the logging provider
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));
            #endregion

            services.AddSingleton<StudentSysDbContext>();

            #region Repository Registration 
            services.AddScoped<IStudentRepository, StudentRepository>();
            #endregion

            return services;
        }
    }
}
