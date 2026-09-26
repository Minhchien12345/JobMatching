using JobMatching.API.DTOs;
using JobMatching.API.Models;
using JobMatching.API.Repositories;

namespace JobMatching.API.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateProfileRepository
            _profileRepository;

        private readonly ICandidateSkillRepository
            _candidateSkillRepository;

        private readonly ISkillRepository _skillRepository;

        public CandidateService(
            ICandidateProfileRepository profileRepository,
            ICandidateSkillRepository candidateSkillRepository,
            ISkillRepository skillRepository
        )
        {
            _profileRepository = profileRepository;
            _candidateSkillRepository =
                candidateSkillRepository;
            _skillRepository = skillRepository;
        }

        public async Task<CandidateProfileResponseDto?>
            GetProfileAsync(int userId)
        {
            CandidateProfile? profile =
                await _profileRepository
                    .GetByUserIdAsync(userId);

            return profile == null
                ? null
                : MapProfile(profile);
        }

        public async Task<CandidateProfileResponseDto>
            UpdateProfileAsync(
                int userId,
                UpdateCandidateProfileDto dto
            )
        {
            CandidateProfile? profile =
                await _profileRepository
                    .GetByUserIdAsync(userId);

            if (profile == null)
            {
                profile = new CandidateProfile
                {
                    UserId = userId,
                    Headline = dto.Headline.Trim(),
                    Bio = dto.Bio.Trim(),
                    Location = dto.Location.Trim(),
                    Education = dto.Education.Trim(),
                    YearsOfExperience =
                        dto.YearsOfExperience
                };

                await _profileRepository.CreateAsync(profile);

                profile = await _profileRepository
                    .GetByUserIdAsync(userId);
            }
            else
            {
                profile.Headline = dto.Headline.Trim();
                profile.Bio = dto.Bio.Trim();
                profile.Location = dto.Location.Trim();
                profile.Education = dto.Education.Trim();
                profile.YearsOfExperience =
                    dto.YearsOfExperience;

                await _profileRepository.UpdateAsync(profile);
            }

            return MapProfile(profile!);
        }

        public async Task<List<CandidateSkillResponseDto>>
            GetSkillsAsync(int userId)
        {
            List<CandidateSkill> candidateSkills =
                await _candidateSkillRepository
                    .GetAllAsync(userId);

            return candidateSkills
                .Select(MapSkill)
                .ToList();
        }

        public async Task<CandidateSkillResponseDto?>
            AddSkillAsync(
                int userId,
                AddCandidateSkillDto dto
            )
        {
            Skill? skill =
                await _skillRepository.GetByIdAsync(
                    dto.SkillId
                );

            if (skill == null)
            {
                return null;
            }

            CandidateSkill? existing =
                await _candidateSkillRepository.GetAsync(
                    userId,
                    dto.SkillId
                );

            if (existing != null)
            {
                return null;
            }

            var candidateSkill = new CandidateSkill
            {
                UserId = userId,
                SkillId = dto.SkillId,
                ProficiencyLevel =
                    dto.ProficiencyLevel,
                YearsOfExperience =
                    dto.YearsOfExperience,
                Skill = skill
            };

            await _candidateSkillRepository.CreateAsync(
                candidateSkill
            );

            return MapSkill(candidateSkill);
        }

        public async Task<CandidateSkillResponseDto?>
            UpdateSkillAsync(
                int userId,
                int skillId,
                UpdateCandidateSkillDto dto
            )
        {
            CandidateSkill? candidateSkill =
                await _candidateSkillRepository.GetAsync(
                    userId,
                    skillId
                );

            if (candidateSkill == null)
            {
                return null;
            }

            candidateSkill.ProficiencyLevel =
                dto.ProficiencyLevel;

            candidateSkill.YearsOfExperience =
                dto.YearsOfExperience;

            await _candidateSkillRepository.UpdateAsync(
                candidateSkill
            );

            return MapSkill(candidateSkill);
        }

        public async Task<bool> DeleteSkillAsync(
            int userId,
            int skillId
        )
        {
            CandidateSkill? candidateSkill =
                await _candidateSkillRepository.GetAsync(
                    userId,
                    skillId
                );

            if (candidateSkill == null)
            {
                return false;
            }

            await _candidateSkillRepository.DeleteAsync(
                candidateSkill
            );

            return true;
        }

        private static CandidateProfileResponseDto
            MapProfile(CandidateProfile profile)
        {
            return new CandidateProfileResponseDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                FullName = profile.User.FullName,
                Email = profile.User.Email,
                Headline = profile.Headline,
                Bio = profile.Bio,
                Location = profile.Location,
                Education = profile.Education,
                YearsOfExperience =
                    profile.YearsOfExperience
            };
        }

        private static CandidateSkillResponseDto MapSkill(
            CandidateSkill candidateSkill
        )
        {
            return new CandidateSkillResponseDto
            {
                SkillId = candidateSkill.SkillId,
                SkillName = candidateSkill.Skill.Name,
                Category = candidateSkill.Skill.Category,
                ProficiencyLevel =
                    candidateSkill.ProficiencyLevel,
                YearsOfExperience =
                    candidateSkill.YearsOfExperience
            };
        }
    }
}