using JobMatching.API.Models;

namespace JobMatching.API.Repositories
{
    public interface ICandidateProfileRepository
    {
        Task<CandidateProfile?> GetByUserIdAsync(int userId);

        Task<CandidateProfile> CreateAsync(
            CandidateProfile profile
        );

        Task UpdateAsync(CandidateProfile profile);
    }
}