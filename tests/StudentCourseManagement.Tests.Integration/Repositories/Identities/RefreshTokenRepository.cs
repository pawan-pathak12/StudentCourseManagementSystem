using System.Transactions;

namespace StudentCourseManagement.Tests.Integration.Repositories.Identities
{
    [DoNotParallelize]
    public class RefreshTokenRepository
    {
        private TransactionScope _scope;

        [TestInitialize]
        public void Init()
        {
            _scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _scope.Dispose(); // rollback
        }

    }
}
