using JobMatching.API.Models;

namespace JobMatching.API.Services
{
    public interface IJwtTokenService
    {
        (string Token, DateTime ExpiresAt) GenerateToken(
            User user
        );
    }
}