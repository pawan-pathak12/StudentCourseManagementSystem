using Microsoft.Extensions.Configuration;
using StudentCourseManagement.Data.Database;

namespace StudentCourseManagement.Tests.Integration
{
    public class DatabaseFixture : IDisposable
    {
        public IConfiguration Configuration { get; }
        public StudentSysDbContext DbContext { get; }

        public DatabaseFixture()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json", optional: false)
                .Build();

            DbContext = new StudentSysDbContext(Configuration);

        }

        public void Dispose()
        {

        }
    }
}
