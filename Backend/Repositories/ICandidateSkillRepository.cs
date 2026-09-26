using JobMatching.API.Models;

namespace JobMatching.API.Repositories
{
    public interface ICandidateSkillRepository
    {
        Task<List<CandidateSkill>> GetAllAsync(int userId);

        Task<CandidateSkill?> GetAsync(
            int userId,
            int skillId
        );

        Task<CandidateSkill> CreateAsync(
            CandidateSkill candidateSkill
        );

        Task UpdateAsync(CandidateSkill candidateSkill);

        Task DeleteAsync(CandidateSkill candidateSkill);
    }
}