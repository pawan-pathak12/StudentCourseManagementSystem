using Microsoft.Extensions.DependencyInjection;
using StudentCourseManagement.Application.DTOs.Auth;
using StudentCourseManagement.Business.DTOs.Auth;
using StudentCourseManagement.Domain.Entities.Identites;
using StudentCourseManagement.Tests.Api.Fixtures;
using StudentCourseManagement.Tests.Common;
using System.Data;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Transactions;

namespace StudentCourseManagement.Tests.Api
{
    [TestClass]
    public abstract class IntegrationTestBase
    {
        protected static CustomWebApplicationFactory Factory = null!;
        protected HttpClient _client = null!;
        protected IDbConnection Connection = null!;
        private TransactionScope _scope = null!;

        #region Class Init

        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void ClassInitialize(TestContext context)
        {
            Factory = new CustomWebApplicationFactory();
        }

        [ClassCleanup(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void ClassCleanup()
        {
            Factory.Dispose();
        }

        #endregion

        #region Test Init

        [TestInitialize]
        public async Task TestInitialize()
        {
            _scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            _client = Factory.CreateClient();

            var scope = Factory.Services.CreateScope();
            Connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();

            var user = await CreateUser();
            var token = JwtTestTokenGenerator.GenerateToken(user);

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _client.Dispose();
            _scope.Dispose(); // rollback
        }

        #endregion

        #region Helpers

        protected async Task<User> CreateUser()
        {
            var user = new RegisterRequest
            {
                Email = Guid.NewGuid() + "@gmail.com",
                Password = "Apple@@211"
            };

            var response = await _client.PostAsJsonAsync("/api/Auth/register", user);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<UserResponseDto>();

            return new User
            {
                UserId = data!.UserId,
                Email = data.Email,
                Role = data.Role
            };
        }

        #endregion
    }
}
