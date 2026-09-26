using JobMatching.API.Models;

namespace JobMatching.API.Repositories
{
    public interface IJobRepository
    {
        Task<List<Job>> SearchOpenAsync(
            string? keyword,
            string? location
        );

        Task<Job?> GetByIdAsync(int id);

        Task<List<Job>> GetByRecruiterIdAsync(
            int recruiterId
        );

        Task<Job> CreateAsync(Job job);

        Task UpdateAsync(Job job);
    }
}