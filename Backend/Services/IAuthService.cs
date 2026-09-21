using JobMatching.API.DTOs;

namespace JobMatching.API.Services
{
    public interface IAuthService
    {
        Task<UserResponseDto?> RegisterAsync(RegisterDto dto);
    }
}