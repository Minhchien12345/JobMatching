using JobMatching.API.DTOs;
using JobMatching.API.Enums;
using JobMatching.API.Models;
using JobMatching.API.Repositories;
using Microsoft.AspNetCore.Identity;

namespace JobMatching.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IJwtTokenService jwtTokenService
        )
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<UserResponseDto?> RegisterAsync(
            RegisterDto dto
        )
        {
            string normalizedEmail =
                dto.Email.Trim().ToLowerInvariant();

            bool emailExists =
                await _userRepository.EmailExistsAsync(
                    normalizedEmail
                );

            if (emailExists)
            {
                return null;
            }

            var user = new User
            {
                FullName = dto.FullName.Trim(),
                Email = normalizedEmail,
                Role = UserRole.Candidate,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash =
                _passwordHasher.HashPassword(
                    user,
                    dto.Password
                );

            User createdUser =
                await _userRepository.CreateAsync(user);

            return MapToUserResponse(createdUser);
        }

        public async Task<LoginResponseDto?> LoginAsync(
            LoginDto dto
        )
        {
            string normalizedEmail =
                dto.Email.Trim().ToLowerInvariant();

            User? user =
                await _userRepository.GetByEmailAsync(
                    normalizedEmail
                );

            if (user == null)
            {
                return null;
            }

            if (!user.IsActive)
            {
                return null;
            }

            PasswordVerificationResult passwordResult =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    dto.Password
                );

            if (
                passwordResult
                == PasswordVerificationResult.Failed
            )
            {
                return null;
            }

            var tokenResult =
                _jwtTokenService.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = tokenResult.Token,
                ExpiresAt = tokenResult.ExpiresAt,
                User = MapToUserResponse(user)
            };
        }

        private static UserResponseDto MapToUserResponse(
            User user
        )
        {
            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}