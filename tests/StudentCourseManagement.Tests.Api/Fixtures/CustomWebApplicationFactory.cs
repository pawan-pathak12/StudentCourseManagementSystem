using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace StudentCourseManagement.Tests.Api.Fixtures
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Optional: Replace services with test doubles
                // Example: Replace real database with in-memory or test database

                // var descriptor = services.SingleOrDefault(
                //     d => d.ServiceType == typeof(DbContextOptions<YourDbContext>));
                // if (descriptor != null)
                //     services.Remove(descriptor);

                // services.AddDbContext<YourDbContext>(options =>
                // {
                //     options.UseInMemoryDatabase("TestDb");
                // });
            });
        }
    }
}
