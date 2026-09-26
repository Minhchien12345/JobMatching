using JobMatching.API.Models;

namespace JobMatching.API.Repositories
{
    public interface IApplicationRepository
    {
        Task<JobApplication?> GetByIdAsync(int id);

        Task<JobApplication?> GetByJobAndCandidateAsync(
            int jobId,
            int candidateId
        );

        Task<List<JobApplication>> GetByCandidateIdAsync(
            int candidateId
        );

        Task<List<JobApplication>> GetByJobIdAsync(int jobId);

        Task<JobApplication> CreateAsync(
            JobApplication application
        );

        Task UpdateAsync(JobApplication application);
    }
}