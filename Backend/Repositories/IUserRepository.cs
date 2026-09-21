using JobMatching.API.Models;

namespace JobMatching.API.Repositories
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email);

        Task<User> CreateAsync(User user);
    }
}