using StudentCourseManagement.Application.DTOs.Auth;
using StudentCourseManagement.Business.DTOs.Auth;
using StudentCourseManagement.Domain.Entities.Identites;
using StudentCourseManagement.Tests.Api.Fixtures;
using StudentCourseManagement.Tests.Common;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Transactions;

namespace StudentCourseManagement.Tests.Api
{
    [TestClass]
    public abstract class IntegrationTestBase
    {
        private TransactionScope _scope = null!;
        private static CustomWebApplicationFactory _factory = null!;
        private HttpClient _client = null!;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _factory = new CustomWebApplicationFactory();
        }

        [TestInitialize]
        public async Task TestInit()
        {
            _scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            _client = _factory.CreateClient();

            var user = await CreateUser();

            var token = JwtTestTokenGenerator.GenerateToken(user);
            _client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", token);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _factory.Dispose();
        }
        [TestCleanup]
        public void TestCleanUp()
        {
            _client.Dispose();
            _scope.Dispose();
        }

        #region helper method
        public async Task<User> CreateUser()
        {
            var user = new RegisterRequest
            {
                Email = "user111@gmail.com",
                Password = "Apple@@211"
            };
            var userResponse = await _client.PostAsJsonAsync("/api/Auth/register", user);

            userResponse.EnsureSuccessStatusCode();
            var userData = await userResponse.Content
                .ReadFromJsonAsync<UserResponseDto>();

            return new User
            {
                UserId = userData.UserId,
                Email = userData?.Email,
                Role = userData?.Role
            };
        }
        #endregion
    }
}
