using LibraryManagementSystem.BLL.Services.Interfaces;
using LibraryManagementSystem.DOL.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result == null)
                return Unauthorized(ApiResponse<string>.Fail("Invalid credentials"));
            return Ok(ApiResponse<AuthResponseDto>.Ok(result, "Login successful"));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (result == null)
                return BadRequest(ApiResponse<string>.Fail("Email already exists"));
            return Ok(ApiResponse<AuthResponseDto>.Ok(result, "Registration successful"));
        }
    }
}