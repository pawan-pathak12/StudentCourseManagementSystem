using Dapper;
using Microsoft.Extensions.Logging;
using StudentCourseManagement.Business.Interfaces.Repositories.Identity;
using StudentCourseManagement.Data.Database;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Data.Repositories.Dapper.Identity
{
    public class UserRepository : IUserRepository
    {
        private readonly StudentSysDbContext _dbContext;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(StudentSysDbContext dbContext, ILogger<UserRepository> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }
        public async Task<int> AddAsync(User user)
        {
            const string sql = @"INSERT INTO Users (Email, PasswordHash, CreatedAt)
                        VALUES (@Email, @PasswordHash, @CreatedAt);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = _dbContext.CreateConnection();
            var userId = await connection.QuerySingleAsync<int>(sql, user);
            _logger.LogInformation($"new user created with Id {userId} ");
            return userId;
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            const string sql = @"select case when 
                            exists (select 1 from Users where Email=@Email) 
                            then 1 else 0 end";
            using var connection = _dbContext.CreateConnection();
            var result = await connection.QuerySingleAsync<bool>(sql, new { Email = email });
            if (!result)
            {
                _logger.LogWarning("Email don't exists");
            }
            return result;
        }

        public async Task<User?> GetByEmailAddressAsync(string? email)
        {
            const string sql = @"Select * from Users where Email=@Email";
            using var connection = _dbContext.CreateConnection();
            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
            if (user == null)
            {
                _logger.LogWarning("Email don't exists");
            }
            return user;
        }
    }
}
