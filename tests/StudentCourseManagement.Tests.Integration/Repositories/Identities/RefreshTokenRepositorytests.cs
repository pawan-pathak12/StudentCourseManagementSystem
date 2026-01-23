
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using StudentCourseManagement.Business.Interfaces.Repositories.Identities;
using StudentCourseManagement.Business.Services;
using StudentCourseManagement.Data.Repositories.Dapper.Identities;
using StudentCourseManagement.Domain.Entities;
using System.Transactions;

namespace StudentCourseManagement.Tests.Integration.Repositories.Identities
{
    [TestClass]
    [DoNotParallelize]
    public class RefreshTokenRepositorytests
    {
        private TransactionScope? _scope;
        private RefreshTokenRepository _refreshTokenRepository;
        private UserRepository _userRepository;
        private AuthService _authService;

        [TestInitialize]
        public void Init()
        {
            _scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var dbfixture = new DatabaseFixture();
            var mockLogger = new Mock<ILogger<UserRepository>>();
            var mockConf = new Mock<IConfiguration>();
            var mockIUserRepo = new Mock<IUserRepository>();
            _authService = new AuthService(mockConf.Object, mockIUserRepo.Object);
            _userRepository = new UserRepository(dbfixture.DbContext, mockLogger.Object);
            _refreshTokenRepository = new RefreshTokenRepository(dbfixture.DbContext);
        }



        [TestCleanup]
        public void Cleanup()
        {
            _scope?.Dispose(); // rollback
        }

        #region CURD Operations 
        [TestMethod]
        public async Task AddAsync_WithValidDate_ReturnCreatedId()
        {
            //Arrange 
            var userId = await CreateUserAsync();
            //Act 
            var refreshTokenId = await CreateRefreshTokenAsync(userId);
            //Assert
            Assert.IsGreaterThan(0, refreshTokenId);
        }

        [TestMethod]
        public async Task GetRefreshTokenWithUserAsync_WithExistingToekn_ReturnRefreshTokenAndUserEntity()
        {
            //Arrange 
            var userId = await CreateUserAsync();
            var refreshTokenid = await CreateRefreshTokenAsync(userId);
            var refreshToken = await _refreshTokenRepository.GetByIdAsync(refreshTokenid);

            //Act
            var result = await _refreshTokenRepository.GetRefreshTokenWithUserAsync(refreshToken.Token);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.UserId);
            Assert.AreEqual(refreshTokenid, result.RefreshTokenId);

        }

        [TestMethod]
        public async Task GetByTokenAsync_IfExistingRefreshTokenMatched_ThenReturnRefreshTokenEntity()
        {
            //Arrange 
            var userId = await CreateUserAsync();
            var refreshTokenid = await CreateRefreshTokenAsync(userId);
            var refreshToken = await _refreshTokenRepository.GetByIdAsync(refreshTokenid);

            //Act
            var result = await _refreshTokenRepository.GetByTokenAsync(refreshToken.Token);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<RefreshToken>(result);
            Assert.AreEqual(refreshTokenid, result.RefreshTokenId);
        }

        [TestMethod]
        public async Task UpdateAsync_WithExistingData_ReturnTrue()
        {
            //Arrange 
            var userId = await CreateUserAsync();
            var refreshTokenid = await CreateRefreshTokenAsync(userId);
            var updateEntity = new RefreshToken
            {
                RefreshTokenId = refreshTokenid,
                UserId = userId,
                IsRevoked = false,
                Token = _authService.GenerateRefreshToken(),
            };

            //Act
            var result = await _refreshTokenRepository.UpdateAsync(updateEntity);

            //Assert
            Assert.AreEqual(1, result);
            var refreshTokenEntity = await _refreshTokenRepository.GetByIdAsync(refreshTokenid);
            Assert.IsNotNull(refreshTokenEntity);
            Assert.AreEqual(updateEntity.Token, refreshTokenEntity.Token);
        }

        [TestMethod]
        public async Task GetByIdAsync_WithExistingTokenId_ReturnRefreshTokenEntity()
        {
            //Arrange

            var userId = await CreateUserAsync();
            var refreshTokenid = await CreateRefreshTokenAsync(userId);
            //Act 

            var refreshTokenEntity = await _refreshTokenRepository.GetByIdAsync(refreshTokenid);
            //Assert
            Assert.IsNotNull(refreshTokenEntity);
        }

        #endregion


        #region Private Helper Methods 

        public async Task<int> CreateRefreshTokenAsync(int userId)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                CreatedAt = DateTimeOffset.UtcNow,
                IsRevoked = false,
                Token = _authService.GenerateRefreshToken(),

                ExpiresAt = DateTimeOffset.UtcNow.AddDays(7)
            };

            return await _refreshTokenRepository.AddAsync(refreshToken);
        }
        public async Task<int> CreateUserAsync()
        {
            var user = new User
            {
                CreatedAt = DateTimeOffset.UtcNow,
                Email = "pawan@gmail.com",
                PasswordHash = "ifsi0iisfsbgdiuiotrbrig",
                Role = "User"
            };
            return await _userRepository.AddAsync(user);
        }
        #endregion

    }
}
