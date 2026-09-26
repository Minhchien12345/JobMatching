using JobMatching.API.DTOs;
using JobMatching.API.Enums;
using JobMatching.API.Models;
using JobMatching.API.Repositories;

namespace JobMatching.API.Services
{
    public class MatchingService : IMatchingService
    {
        private readonly IJobRepository _jobRepository;

        private readonly ICandidateSkillRepository
            _candidateSkillRepository;

        public MatchingService(
            IJobRepository jobRepository,
            ICandidateSkillRepository candidateSkillRepository
        )
        {
            _jobRepository = jobRepository;
            _candidateSkillRepository =
                candidateSkillRepository;
        }

        public async Task<MatchResultDto?> CalculateAsync(
            int candidateId,
            int jobId
        )
        {
            Job? job =
                await _jobRepository.GetByIdAsync(jobId);

            if (
                job == null
                || job.Status != JobStatus.Open
                || !job.Company.IsActive
            )
            {
                return null;
            }

            List<CandidateSkill> candidateSkills =
                await _candidateSkillRepository.GetAllAsync(
                    candidateId
                );

            return Calculate(job, candidateSkills);
        }

        public async Task<List<MatchResultDto>>
            GetRecommendationsAsync(int candidateId)
        {
            List<Job> jobs =
                await _jobRepository.SearchOpenAsync(
                    null,
                    null
                );

            List<CandidateSkill> candidateSkills =
                await _candidateSkillRepository.GetAllAsync(
                    candidateId
                );

            return jobs
                .Select(job =>
                    Calculate(job, candidateSkills)
                )
                .OrderByDescending(result =>
                    result.MatchScore
                )
                .ThenByDescending(result =>
                    result.MatchedSkillCount
                )
                .ToList();
        }

        private static MatchResultDto Calculate(
            Job job,
            List<CandidateSkill> candidateSkills
        )
        {
            Dictionary<int, CandidateSkill>
                candidateSkillDictionary =
                    candidateSkills.ToDictionary(
                        candidateSkill =>
                            candidateSkill.SkillId
                    );

            var matchedSkills =
                new List<SkillMatchDetailDto>();

            var missingSkills =
                new List<SkillMatchDetailDto>();

            foreach (JobSkill jobSkill in job.JobSkills)
            {
                bool hasSkill =
                    candidateSkillDictionary.TryGetValue(
                        jobSkill.SkillId,
                        out CandidateSkill? candidateSkill
                    );

                var detail = new SkillMatchDetailDto
                {
                    SkillId = jobSkill.SkillId,
                    SkillName = jobSkill.Skill.Name,
                    Category = jobSkill.Skill.Category,
                    RequiredLevel =
                        jobSkill.RequiredLevel,
                    CandidateLevel =
                        candidateSkill?.ProficiencyLevel,

                    MeetsRequiredLevel =
                        hasSkill
                        && candidateSkill!.ProficiencyLevel
                            >= jobSkill.RequiredLevel
                };

                if (hasSkill)
                {
                    matchedSkills.Add(detail);
                }
                else
                {
                    missingSkills.Add(detail);
                }
            }

            int requiredCount = job.JobSkills.Count;
            int matchedCount = matchedSkills.Count;

            decimal matchScore =
                requiredCount == 0
                    ? 0
                    : Math.Round(
                        (decimal)matchedCount
                        / requiredCount
                        * 100,
                        2
                    );

            return new MatchResultDto
            {
                JobId = job.Id,
                JobTitle = job.Title,
                CompanyId = job.CompanyId,
                CompanyName = job.Company.Name,
                RequiredSkillCount = requiredCount,
                MatchedSkillCount = matchedCount,
                MatchScore = matchScore,
                MatchedSkills = matchedSkills,
                MissingSkills = missingSkills
            };
        }
    }
}