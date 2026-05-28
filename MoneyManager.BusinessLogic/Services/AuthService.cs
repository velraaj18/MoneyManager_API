using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MoneyManager.Data;
using MoneyManager.DTO;
using MoneyManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MoneyManager.Services
{
    public class AuthService
    {
        private readonly AppDBContext _dbContext;
        private readonly PasswordHasher<User> _passwordHasher;
        // Strongly typed app setting registered in program.cs
        private readonly JwtSettings _jwtSettings;

        public AuthService(AppDBContext dbContext, PasswordHasher<User> passwordHasher, IOptions<JwtSettings> jwtSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<APIResponse<User>> Register(UserRequest dto)
        {
            if (dto == null)
            {
                return new APIResponse<User> { StatusCode = 400, Message = "You must provide the details", Data = null };
            }

            var existingUser = _dbContext.Users.Where(x => x.Email == dto.Email).FirstOrDefault();
            if (existingUser != null)
            {
                return new APIResponse<User> { StatusCode = 400, Message = "User Already registered", Data = existingUser };
            }

            // Create a new User entity
            var user = new User
            {
                Email = dto.Email
            };

            // Hash the plain password
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return new APIResponse<User>
            {
                StatusCode = 201,
                Message = "User registered successfully",
                Data = user
            };
        }

        public async Task<APIResponse<dynamic>> Login(UserRequest dto)
        {
            if (dto == null)
            {
                return new APIResponse<dynamic> { StatusCode = 400, Message = "You must provide the details", Data = null };
            }

            var user = _dbContext.Users.Where(x => x.Email == dto.Email).FirstOrDefault();
            if (user == null)
            {
                return new APIResponse<dynamic> { StatusCode = 401, Message = "Invalid Email or Password", Data = null };
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result != PasswordVerificationResult.Success)
            {
                return new APIResponse<dynamic> { StatusCode = 401, Message = "Invalid Email or Password", Data = null };
            }

            // --------------- JWT token generation ------------------ //

            // Generate Access token
            var accessToken = GenerateAccessToken(user);

            // Generate a refresh token and store in DB
            var refreshTokenString = GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                Token = refreshTokenString,
                UserId = user.UserUID,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            _dbContext.RefreshTokens.Add(refreshToken);
            await _dbContext.SaveChangesAsync();

            return new APIResponse<dynamic> { StatusCode = 200, Message = "User logged in successfully", Data = new { token = accessToken, refreshToken = refreshTokenString } };
        }

        // So when user asks for refresh token and it is not expired or revoked, we provide them with new access token and refresh token
        public async Task<APIResponse<dynamic>> RefreshToken(RefreshTokenReq dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.RefreshToken))
            {
                return new APIResponse<dynamic> { StatusCode = 400, Message = "You must provide the refresh token", Data = null };
            }

            // Fetch the refresh token from the DB
            var storedToken = await _dbContext.RefreshTokens.Where(x => x.Token == dto.RefreshToken).Include(y => y.User).FirstOrDefaultAsync();
            if (storedToken == null)
            {
                return new APIResponse<dynamic> { StatusCode = 401, Message = "No Refresh Token Found", Data = null };
            }

            if (storedToken.IsRevoked)
            {
                return new APIResponse<dynamic>
                {
                    StatusCode = 401,
                    Message = "Refresh token revoked"
                };
            }

            if (storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return new APIResponse<dynamic>
                {
                    StatusCode = 401,
                    Message = "Refresh token expired"
                };
            }

            // revoke the old token to not use it again
            storedToken.IsRevoked = true;

            // Generate new access token and refresh token
            var accessToken = GenerateAccessToken(storedToken.User);
            var refreshToken = GenerateRefreshToken();

            var newRefreshToken = new RefreshToken
            {
                Token = refreshToken,
                UserId = storedToken.User.UserUID,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.RefreshTokens.Add(newRefreshToken);

            await _dbContext.SaveChangesAsync();

            return new APIResponse<dynamic>
            {
                StatusCode = 200,
                Message = "Token refreshed successfully",
                Data = new
                {
                    token = accessToken,
                    refreshToken = refreshToken
                }
            };
        }
        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        public string GenerateAccessToken(User currentUser)
        {
            // Step 1: Create claims
            var claims = new List<Claim>()
            {
                new (ClaimTypes.Email, currentUser.Email),
                new (ClaimTypes.NameIdentifier, currentUser.UserUID.ToString())
            };

            // Step 2: Create sign key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            // Step 3: Create signing credentials using key and securit algorithm
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Step 4: Create token
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: cred
            );

            // Step 5: convert token object into string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        public async Task Logout(string userId)
        {
            if (!int.TryParse(userId, out int id)) return;

            var refreshTokens = await _dbContext.RefreshTokens
                .Where(x => x.UserId == id)
                .ToListAsync();

            foreach (var token in refreshTokens) token.IsRevoked = true;

            await _dbContext.SaveChangesAsync();
        }
    }
}
