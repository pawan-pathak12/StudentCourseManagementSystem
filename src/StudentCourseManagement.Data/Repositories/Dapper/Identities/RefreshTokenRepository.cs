using Dapper;
using StudentCourseManagement.Application.DTOs.Auth;
using StudentCourseManagement.Business.Interfaces.Repositories.Identities;
using StudentCourseManagement.Data.Database;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.Data.Repositories.Dapper.Identities
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

        public async Task<RefreshTokenWithUserDto?> GetRefreshTokenWithUserAsync(string token)
        {
            const string sql = @"select * 
                            from Users u
                            inner join RefreshTokens rf on u.UserId = rf.UserId
                            where rf.IsRevoked=0 and rf.Token=@Token";

            using var connection = _dbContext.CreateConnection();

            var result = await connection.QueryAsync<RefreshTokenWithUserDto>(sql, new { Token = token });
            return result.FirstOrDefault();

        }
        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            const string sql = @" SELECT * FROM RefreshTokens
                            WHERE Token = @Token AND IsRevoked = 0;";
            using var connection = _dbContext.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<RefreshToken>(sql, new { Token = token });
            return result;
        }
        public async Task<int> UpdateAsync(RefreshToken refreshToken)
        {
            const string sql = @" UPDATE RefreshTokens
                            SET ExpiresAt = @ExpiresAt,
                                RevokedAt = @RevokedAt,
                                IsRevoked = @IsRevoked,
                                Token= @Token,
                                ReplacedByToken = @ReplacedByToken
                            WHERE RefreshTokenId = @RefreshTokenId;";

            using var connection = _dbContext.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(sql, refreshToken);

            return rowsAffected;
        }

        public async Task<RefreshToken?> GetByIdAsync(int refreshTokenId)
        {
            const string sql = @"SELECT RefreshTokenId, UserId, Token, ExpiresAt, CreatedAt 
                         FROM RefreshTokens 
                         WHERE RefreshTokenId = @Id";
            using var connection = _dbContext.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<RefreshToken>(sql, new { Id = refreshTokenId });

        }
    }
}
