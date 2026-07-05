using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Webshop.Api.Data;
using Webshop.Api.DTOs;
using Webshop.Api.Models;
using Webshop.Api.Services.Interfaces;

namespace Webshop.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResult?> RegisterAsync(RegisterDto dto, string ipAddress)
        {
            var emailExists = await _context.Users
                            .AnyAsync(u => u.Email == dto.Email);

            if (emailExists) 
            {
                return null;
            }

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            //var token = GenerateJwtToken(user);

            /*return new AuthResponseDto
            {
                Token = token,
                Email = dto.Email,
                Role = user.Role
            };*/

            return await IssueTokensAsync(user, ipAddress);
        }

        public async Task<AuthResult?> LoginAsync(LoginDto dto, string ipAddress)
        {
            var user = await _context.Users
                     .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null) 
            {
                return null;
            }

            var passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!passwordValid) 
            {
                return null;
            }

            //var token = GenerateJwtToken(user);

            /*return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = user.Role
            };*/

            return await IssueTokensAsync(user, ipAddress);
        }

        public async Task<AuthResult?> RefreshTokenAsync(string refreshToken, string ipAddress)
        {
            var hashedToken = HashToken(refreshToken);

            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.TokenHash == hashedToken);

            // Ismeretlen token - nincs info
            if (storedToken == null)
            {
                return null;
            }

            if (storedToken.IsRevoked)
            {
                // REUSE DETECTION: valaki egy már felhasznált (rotált) refresh tokent
                // próbál újra beváltani. Ez tipikusan token lopásra utal - biztonsági
                // óvintézkedésként a felhasználó ÖSSZES aktív tokenjét visszavonjuk,
                // így minden eszközön/böngészőben újra be kell jelentkeznie.
                await RevokeAllActiveTokensAsync(storedToken.UserId, ipAddress);
                return null;
            }

            if (!storedToken.IsActive)
            {
                // Lejárt, de nem visszavont token - egyszerűen érvénytelen
                return null;
            }

            var newRefreshTokenPlain = GenerateRefreshToken();
            var newRefreshTokenHash = HashToken(newRefreshTokenPlain);

            storedToken.RevokedAt = DateTime.UtcNow;
            storedToken.RevokedByIp = ipAddress;
            storedToken.ReplacedByTokenHash = newRefreshTokenHash;

            var newTokenEntity = new RefreshToken
            {
                TokenHash = newRefreshTokenHash,
                UserId = storedToken.UserId,
                ExpiresAt = DateTime.UtcNow.AddDays(GetRefreshTokenExpiresInDays()),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };

            _context.RefreshTokens.Add(newTokenEntity);
            await _context.SaveChangesAsync();

            var newAccessToken = GenerateJwtToken(storedToken.User);

            return new AuthResult
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenPlain,
                Email = storedToken.User.Email,
                Role = storedToken.User.Role
            };
        }

        public async Task RevokeTokenAsync(string refreshToken, string ipAddress)
        {
            var hashedToken = HashToken(refreshToken);

            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.TokenHash == hashedToken);

            if (storedToken == null || !storedToken.IsActive)
            {
                return;
            }

            storedToken.RevokedAt = DateTime.UtcNow;
            storedToken.RevokedByIp = ipAddress;

            await _context.SaveChangesAsync();
        }

        private async Task<AuthResult> IssueTokensAsync(User user, string ipAddress)
        {
            var accessToken = GenerateJwtToken(user);
            var refreshTokenPlain = GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                TokenHash = HashToken(refreshTokenPlain),
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(GetRefreshTokenExpiresInDays()),
                CreatedAt = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new AuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenPlain,
                Email = user.Email,
                Role = user.Role
            };
        }

        private async Task RevokeAllActiveTokensAsync(int userId, string ipAddress)
        {
            var activeTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.RevokedAt == null && rt.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();

            foreach (var token in activeTokens)
            {
                token.RevokedAt = DateTime.UtcNow;
                token.RevokedByIp = ipAddress;
            }

            await _context.SaveChangesAsync();
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresInMinutes = Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"]);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        private static string HashToken(string token)
        {
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hashBytes);
        }

        private double GetRefreshTokenExpiresInDays()
        {
            var configured = _configuration["Jwt:RefreshTokenExpiresInDays"];
            return configured != null ? Convert.ToDouble(configured) : 7;
        }

        /*private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresInMInutes = Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"]);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMInutes),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        */
    }
}
