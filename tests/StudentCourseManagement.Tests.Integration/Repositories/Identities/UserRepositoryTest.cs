using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Data.Repositories.Dapper.Identities;
using StudentCourseManagement.Domain.Entities.Identites;
using System.Transactions;

namespace StudentCourseManagement.Tests.Integration.Repositories.Identities
{
    [TestClass]
    [DoNotParallelize]
    public class UserRepositoryTest
    {
        private TransactionScope _scope;
        protected UserRepository _userRepository;

        [TestInitialize]
        public void Init()
        {
            _scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }
        [TestCleanup]
        public void CLeanup()
        {
            _scope.Dispose();
        }
        public UserRepositoryTest()
        {
            var fixture = new DatabaseFixture();
            var loggerMock = new Mock<ILogger<UserRepository>>();
            _userRepository = new UserRepository(fixture.DbContext, loggerMock.Object);
        }

        #region CURD Operations 

        [TestMethod]
        public async Task AddAsync_ForRecentCreatedStudent_ReturnUserId()
        {
            //Arrange 

            //Act 
            var userId = await CreateUserAsyc();
            //Assert 
            Assert.IsGreaterThan(0, userId);

        }

        [TestMethod]
        public async Task GetByIdAsync_WihExistingId_ReturnUser()
        {
            //Arrange 
            var userId = await CreateUserAsyc();

            //Act 
            var user = await _userRepository.GetByIdAsync(userId);
            //Assert 
            Assert.IsNotNull(user);

        }
        [TestMethod]
        public async Task CheckEmailExistsAsync_IfExistingEmailExists_ReturnTrue()
        {
            //Arrange 
            var userId = await CreateUserAsyc();
            var user = await _userRepository.GetByIdAsync(userId);

            //Act 
            Assert.IsNotNull(user);
            var result = await _userRepository.CheckEmailExistsAsync(user.Email);
            //Assert 
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnUsers()
        {
            //Arrange 
            await CreateUserAsyc();

            //Act 
            var users = await _userRepository.GetAllAsync();

            //Assert 
            Assert.IsTrue(users.Any());

        }

        [TestMethod]
        public async Task GetByEmailAddressAsync_WithExistingEmail_ReturnUser()
        {
            //Arrange 
            var userId = await CreateUserAsyc();
            var userData = await _userRepository.GetByIdAsync(userId);

            //Act 
            var user = await _userRepository.GetByEmailAddressAsync(userData.Email);

            //Assert 
            Assert.IsNotNull(user);

        }

        #endregion

        #region HelperMethod

        public async Task<int> CreateUserAsyc()
        {
            var user = new User
            {
                CreatedAt = DateTimeOffset.UtcNow,
                Email = "pawan@gmail.com",
                PasswordHash = "ifsi0iisfsbgdiuiotrbrig ",
                Role = "User"
            };
            return await _userRepository.AddAsync(user);
        }
        #endregion
    }
}
