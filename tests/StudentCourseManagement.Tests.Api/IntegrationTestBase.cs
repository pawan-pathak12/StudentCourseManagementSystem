using Microsoft.Extensions.DependencyInjection;
using StudentCourseManagement.Tests.Api.Builders;
using StudentCourseManagement.Tests.Api.Fixtures;
using StudentCourseManagement.Tests.Common;
using System.Net.Http.Headers;
using System.Transactions;

namespace StudentCourseManagement.Tests.Api
{
    [TestClass]
    public abstract class IntegrationTestBase
    {
        protected static CustomWebApplicationFactory Factory = null!;
        protected TestDataBuilder builder = null!;
        protected HttpClient _client = null!;
        protected TransactionScope _scope = null!;
        protected IServiceProvider Services = null!;

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
            Services = scope.ServiceProvider;
            builder = new TestDataBuilder(Services);

            var user = await builder.CreateUser();
            var token = JwtTestTokenGenerator.GenerateToken(user);

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }


        [TestCleanup]
        public void TestCleanup()
        {
            _client.Dispose();
            _scope.Dispose();
        }

        #endregion

        protected int RandomNumberGenerator()
        {
            var rand = new Random();
            return rand.Next(00000, 99999);
        }


    }
}
