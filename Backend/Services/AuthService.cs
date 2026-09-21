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

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher
        )
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
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

            return new UserResponseDto
            {
                Id = createdUser.Id,
                FullName = createdUser.FullName,
                Email = createdUser.Email,
                Role = createdUser.Role.ToString(),
                IsActive = createdUser.IsActive,
                CreatedAt = createdUser.CreatedAt
            };
        }
    }
}