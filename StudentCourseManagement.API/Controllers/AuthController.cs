using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using StudentCourseManagement.Application.DTOs.DTOs.Auth;
using StudentCourseManagement.Business.Interfaces.Repositories.Identity;
using StudentCourseManagement.Business.Services;
using StudentCourseManagement.Domain.Entities;

namespace StudentCourseManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthController(IAuthService authService, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IPasswordHasher<User> passwordHasher)
        {
            this._authService = authService;
            this._userRepository = userRepository;
            this._refreshTokenRepository = refreshTokenRepository;
            this._passwordHasher = passwordHasher;
        }

        #region HttpPost - Register User
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userExists = await _userRepository.CheckEmailExistsAsync(request.Email);
            if (userExists)
            {
                return BadRequest("User already exists");
            }

            var user = new User
            {
                Email = request.Email
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _userRepository.AddAsync(user);

            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, request);

        }

        #endregion

        #region httpGet - EndPoint 
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        #endregion

        #region Login -EndPoint 
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAddressAsync(request.Email);
            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            // hash the enter password and check it to save db passwordhash
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid credentials");
            }

            // generate access token (jwt)
            var accesstoken = _authService.GenerateJwtToken(user);

            // generate Refresh token
            var refreshToken = new RefreshToken
            {
                UserId = user.UserId,
                Token = _authService.GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            // save to db 
            await _refreshTokenRepository.AddAsync(refreshToken);

            return Ok(new { accessToken = accesstoken, refreshToken = refreshToken });

        }
        #endregion

        #region Refresh Token 

        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            // check if token exists in db or not 
            var storedToken = await _refreshTokenRepository.GetRefreshTokenWithUserAsync(request.RefreshToken);
            if (storedToken == null)
            {
                return Unauthorized("Invalid refresh token");
            }

            //check expiry date of token 
            if (storedToken.ExpiresAt < DateTime.UtcNow)
            {
                return Unauthorized("Refresh token expired");
            }
            var user = storedToken?.User;
            storedToken.IsRevoked = true;

            //generate new token 
            var accessToken = _authService.GenerateJwtToken(user);

            var newRefrshToken = new RefreshToken
            {
                UserId = user.UserId,
                Token = _authService.GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };
            await _refreshTokenRepository.AddAsync(newRefrshToken);

            return Ok(new
            {
                accesstoken = accessToken,
                refreshToken = newRefrshToken.Token
            });
        }
        #endregion

        #region Logout EndPoint 
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string token)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(token);
            if (refreshToken == null)
            {
                return Ok("Token already invalidated");
            }
            refreshToken.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(refreshToken);
            return Ok("Logged Out Successfully");
        }
        #endregion

    }
}
