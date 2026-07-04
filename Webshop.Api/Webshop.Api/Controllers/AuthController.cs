using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webshop.Api.DTOs;
using Webshop.Api.Services.Interfaces;

namespace Webshop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if(result == null)
            {
                return BadRequest("Már létező email.");
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto) 
        {
            var result = await _authService.LoginAsync(dto);

            if (result == null) 
            {
                return Unauthorized("Helytelen Email vagy Jelszó");
            }

            return Ok(result);
        }
    }
}
