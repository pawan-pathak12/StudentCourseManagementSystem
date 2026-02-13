using Microsoft.IdentityModel.Tokens;
using StudentCourseManagement.Domain.Entities.Identites;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentCourseManagement.Tests.Common
{
    public static class JwtTestTokenGenerator
    {
        public static string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("THIS_IS_A_SUPER_SECRET_KEY_CHANGE_LATER_BYOWNER")
                );
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email ,user.Email) ,
                new Claim(ClaimTypes.Role, user.Role) ,
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "JwtAuthLearning"
                , audience: "JwtAuthLearningUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
