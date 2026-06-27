using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URLShortenerAPI.DTOs;
using URLShortenerAPI.Service;

namespace URLShortenerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // -------------------------
        // REGISTER
        // -------------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
        {
            var result = await _authService.RegisterAsync(dto);

            return result.Status == APIStatus.Success
                ? Ok(result)
                : BadRequest(result);
        }

        // -------------------------
        // LOGIN (PASSWORD)
        // -------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto)
        {
            var result = await _authService.LoginAsync(dto);

            return result.Status == APIStatus.Success
                ? Ok(result)
                : BadRequest(result);
        }

        // -------------------------
        // GOOGLE LOGIN
        // -------------------------
        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDTO dto)
        {
            var result = await _authService.GoogleLoginAsync(dto);

            return result.Status == APIStatus.Success
                ? Ok(result)
                : BadRequest(result);
        }
    }
}
