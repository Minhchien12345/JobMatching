using System.Security.Claims;
using JobMatching.API.DTOs;
using JobMatching.API.Services;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(
            LoginDto dto
        )
        {
            LoginResponseDto? result =
                await _authService.LoginAsync(dto);

            if (result == null)
            {
                return Unauthorized(new
                {
                    message =
                        "Email or password is incorrect."
                });
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            return Ok(new
            {
                id = User.FindFirstValue(
                    ClaimTypes.NameIdentifier
                ),

                fullName = User.FindFirstValue(
                    ClaimTypes.Name
                ),

                email = User.FindFirstValue(
                    ClaimTypes.Email
                ),

                role = User.FindFirstValue(
                    ClaimTypes.Role
                )
            });
        }
    }
}