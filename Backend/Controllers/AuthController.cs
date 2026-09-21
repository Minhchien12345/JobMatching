using JobMatching.API.DTOs;
using JobMatching.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobMatching.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register(
            RegisterDto dto
        )
        {
            UserResponseDto? createdUser =
                await _authService.RegisterAsync(dto);

            if (createdUser == null)
            {
                return Conflict(new
                {
                    message = "Email already exists."
                });
            }

            return StatusCode(
                StatusCodes.Status201Created,
                createdUser
            );
        }
    }
}