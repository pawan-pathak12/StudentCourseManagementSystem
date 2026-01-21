using Dapper;
using StudentCourseManagement.Business.Interfaces.Repositories.Identity;
using StudentCourseManagement.Data.Database;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Data.Repositories.Dapper.Identity
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly StudentSysDbContext _dbContext;

        public RefreshTokenRepository(StudentSysDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<int> AddAsync(RefreshToken refreshToken)
        {
            const string sql = @" INSERT INTO RefreshTokens 
                            (Token, UserId, ExpiresAt, CreatedAt, RevokedAt, IsRevoked, ReplacedByToken)
                              VALUES 
                            (@Token, @UserId, @ExpiresAt, @CreatedAt, @RevokedAt, @IsRevoked, @ReplacedByToken);
                             SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = _dbContext.CreateConnection();
            var newId = await connection.QuerySingleAsync<int>(sql, refreshToken);
            return newId;

        }

        public async Task<string> GetStoredTokenAsync(string token)
        {
            const string sql = @" SELECT Token
                             FROM RefreshTokens
                              WHERE Token = @Token
                              AND IsRevoked = 0;";

            using var connection = _dbContext.CreateConnection();

            var StoredToken = await connection.QueryFirstOrDefaultAsync<string>(sql, new { Token = token });
            return token;


        }
    }
}
