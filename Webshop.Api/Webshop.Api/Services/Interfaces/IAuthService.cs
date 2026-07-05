using Webshop.Api.DTOs;

namespace Webshop.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult?> RegisterAsync(RegisterDto dto, string ipAddress);
        Task<AuthResult?> LoginAsync(LoginDto dto, string ipAddress);
        Task<AuthResult?> RefreshTokenAsync(string refreshToken, string ipAddress);
        Task RevokeTokenAsync(string refreshToken, string ipAddress);
    }
}
