using JobMatching.API.DTOs;

namespace JobMatching.API.Services
{
    public interface ICandidateService
    {
        Task<CandidateProfileResponseDto?>
            GetProfileAsync(int userId);

        Task<CandidateProfileResponseDto>
            UpdateProfileAsync(
                int userId,
                UpdateCandidateProfileDto dto
            );

        Task<List<CandidateSkillResponseDto>>
            GetSkillsAsync(int userId);

        Task<CandidateSkillResponseDto?>
            AddSkillAsync(
                int userId,
                AddCandidateSkillDto dto
            );

        Task<CandidateSkillResponseDto?>
            UpdateSkillAsync(
                int userId,
                int skillId,
                UpdateCandidateSkillDto dto
            );

        Task<bool> DeleteSkillAsync(
            int userId,
            int skillId
        );
    }
}