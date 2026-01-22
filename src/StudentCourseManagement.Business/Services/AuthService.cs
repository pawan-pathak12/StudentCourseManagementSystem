using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentCourseManagement.Business.Interfaces.Repositories.Identities;
using StudentCourseManagement.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StudentCourseManagement.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration configuration, IUserRepository userRepository)
        {
            this._configuration = configuration;
            this._userRepository = userRepository;
        }
        public string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");


            //1.claims 
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub ,user.UserId.ToString()) ,
                new Claim(JwtRegisteredClaimNames.Email ,user.Email) ,
                  new Claim (ClaimTypes.Role , user.Role),
                  new Claim(JwtRegisteredClaimNames.Jti ,Guid.NewGuid().ToString())

            };
            //2. prove that : their is use of righr jwt secret key to generate token
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["key"]!)
                );

            //3. Credentials 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(jwtSettings["ExpiresInMinutes"]!)
                    ),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}
