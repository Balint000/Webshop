using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webshop.Api.DTOs;
using Webshop.Api.Services;
using Webshop.Api.Services.Interfaces;

namespace Webshop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private const string RefreshTokenCookieName = "refreshToken";
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto, GetIpAddress());

            if(result == null)
            {
                return BadRequest("Már létező email.");
            }

            SetRefreshTokenCookie(result.RefreshToken);

            return Ok(ToResponseDto(result));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto, [FromHeader(Name = "X-Client-Type")] string? clientType) 
        {
            var result = await _authService.LoginAsync(dto, GetIpAddress());

            if (result == null) 
            {
                return Unauthorized("Helytelen Email vagy Jelszó");
            }

            if (clientType == "desktop")
            {
                // admin desktophoz
                // nincs cookie, a refresh token a JSON body-ban megy,
                // a desktop app felelőssége biztonságosan tárolni
                return Ok(new
                {
                    AccessToken = result.AccessToken,
                    RefreshToken = result.RefreshToken,
                    Email = result.Email,
                    Role = result.Role
                });
            }

            SetRefreshTokenCookie(result.RefreshToken);

            return Ok(ToResponseDto(result));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto? body, [FromHeader(Name = "X-Client-Type")] string? clientType)
        {
            var refreshToken = clientType == "desktop"
                        ? body?.RefreshToken
                        : Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Hiányzó refresh token.");
            }

            var result = await _authService.RefreshTokenAsync(refreshToken, GetIpAddress());

            if (result == null)
            {
                // Érvénytelen, lejárt, vagy újrafelhasznált token - cookie törlése kényszerítve
                DeleteRefreshTokenCookie();
                return Unauthorized("Érvénytelen vagy lejárt refresh token.");
            }

            SetRefreshTokenCookie(result.RefreshToken);

            return Ok(ToResponseDto(result));
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies[RefreshTokenCookieName];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _authService.RevokeTokenAsync(refreshToken, GetIpAddress());
            }

            DeleteRefreshTokenCookie();

            return NoContent();
        }

        private static AuthResponseDto ToResponseDto(AuthResult result)
        {
            return new AuthResponseDto
            {
                Token = result.AccessToken,
                Email = result.Email,
                Role = result.Role
            };
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            Response.Cookies.Append(RefreshTokenCookieName, refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                Path = "/api/auth"
            });
        }

        private void DeleteRefreshTokenCookie()
        {
            Response.Cookies.Delete(RefreshTokenCookieName, new CookieOptions
            {
                Path = "/api/auth"
            });
        }

        private string GetIpAddress()
        {
            if (Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor)
                && !string.IsNullOrWhiteSpace(forwardedFor))
            {
                return forwardedFor.ToString().Split(',')[0].Trim();
            }

            return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }
    }
}
