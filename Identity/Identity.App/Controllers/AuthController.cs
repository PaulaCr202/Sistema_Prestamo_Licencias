using Identity.App.DTOs.Auth;
using Identity.App.Responses;
using Identity.App.Services.Abstractions;
using Identity.App.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Identity.App.Controllers
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            AuthResponseDTO? result = await _authService.LoginAsync(dto);

            if (result is null)
            {
                return Unauthorized(ApiResponse<AuthResponseDTO>.Fail(
                    [new ApiError("Auth.InvalidCredentials", "Correo o contraseña incorrectos.", "unauthorized")]));
            }

            return Ok(ApiResponse<AuthResponseDTO>.Ok(result));
        }
    }
}
