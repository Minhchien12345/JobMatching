using JobMatching.API.DTOs;
using JobMatching.API.Enums;
using JobMatching.API.Models;
using JobMatching.API.Repositories;

namespace JobMatching.API.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ISkillRepository _skillRepository;

        public JobService(
            IJobRepository jobRepository,
            ICompanyRepository companyRepository,
            ISkillRepository skillRepository
        )
        {
            _jobRepository = jobRepository;
            _companyRepository = companyRepository;
            _skillRepository = skillRepository;
        }

        public async Task<List<JobResponseDto>>
            SearchOpenAsync(
                string? keyword,
                string? location
            )
        {
            List<Job> jobs =
                await _jobRepository.SearchOpenAsync(
                    keyword,
                    location
                );

            return jobs.Select(Map).ToList();
        }

        public async Task<JobResponseDto?>
            GetPublicByIdAsync(int id)
        {
            Job? job = await _jobRepository.GetByIdAsync(id);

            if (
                job == null
                || job.Status != JobStatus.Open
                || !job.Company.IsActive
            )
            {
                return null;
            }

            return Map(job);
        }

        public async Task<List<JobResponseDto>>
            GetMineAsync(int recruiterId)
        {
            List<Job> jobs =
                await _jobRepository
                    .GetByRecruiterIdAsync(recruiterId);

            return jobs.Select(Map).ToList();
        }

        public async Task<JobResponseDto?> CreateAsync(
            int recruiterId,
            CreateJobDto dto
        )
        {
            Company? company =
                await _companyRepository
                    .GetByRecruiterIdAsync(recruiterId);

            if (company == null || !company.IsActive)
            {
                return null;
            }

            if (!IsSalaryValid(
                dto.MinSalary,
                dto.MaxSalary
            ))
            {
                return null;
            }

            List<JobSkill>? jobSkills =
                await BuildJobSkillsAsync(
                    dto.RequiredSkills
                );

            if (jobSkills == null)
            {
                return null;
            }

            var job = new Job
            {
                CompanyId = company.Id,
                Title = dto.Title.Trim(),
                Description = dto.Description.Trim(),
                Location = dto.Location.Trim(),
                EmploymentType = dto.EmploymentType,
                MinSalary = dto.MinSalary,
                MaxSalary = dto.MaxSalary,
                Deadline = dto.Deadline,
                Status = JobStatus.Open,
                CreatedAt = DateTime.UtcNow,
                JobSkills = jobSkills
            };

            await _jobRepository.CreateAsync(job);

            Job? createdJob =
                await _jobRepository.GetByIdAsync(job.Id);

            return Map(createdJob!);
        }

        public async Task<JobResponseDto?> UpdateAsync(
            int recruiterId,
            int jobId,
            UpdateJobDto dto
        )
        {
            Job? job =
                await _jobRepository.GetByIdAsync(jobId);

            if (
                job == null
                || job.Company.RecruiterId != recruiterId
            )
            {
                return null;
            }

            if (!IsSalaryValid(
                dto.MinSalary,
                dto.MaxSalary
            ))
            {
                return null;
            }

            List<JobSkill>? jobSkills =
                await BuildJobSkillsAsync(
                    dto.RequiredSkills
                );

            if (jobSkills == null)
            {
                return null;
            }

            job.Title = dto.Title.Trim();
            job.Description = dto.Description.Trim();
            job.Location = dto.Location.Trim();
            job.EmploymentType = dto.EmploymentType;
            job.MinSalary = dto.MinSalary;
            job.MaxSalary = dto.MaxSalary;
            job.Deadline = dto.Deadline;

            job.JobSkills.Clear();

            foreach (JobSkill jobSkill in jobSkills)
            {
                job.JobSkills.Add(jobSkill);
            }

            await _jobRepository.UpdateAsync(job);

            Job? updatedJob =
                await _jobRepository.GetByIdAsync(job.Id);

            return Map(updatedJob!);
        }

        public async Task<bool> CloseAsync(
            int recruiterId,
            int jobId
        )
        {
            Job? job =
                await _jobRepository.GetByIdAsync(jobId);

            if (
                job == null
                || job.Company.RecruiterId != recruiterId
            )
            {
                return false;
            }

            job.Status = JobStatus.Closed;

            await _jobRepository.UpdateAsync(job);

            return true;
        }

        private async Task<List<JobSkill>?>
            BuildJobSkillsAsync(
                List<JobSkillInputDto> skillDtos
            )
        {
            bool hasDuplicate =
                skillDtos
                    .GroupBy(item => item.SkillId)
                    .Any(group => group.Count() > 1);

            if (hasDuplicate)
            {
                return null;
            }

            var result = new List<JobSkill>();

            foreach (JobSkillInputDto dto in skillDtos)
            {
                Skill? skill =
                    await _skillRepository.GetByIdAsync(
                        dto.SkillId
                    );

                if (skill == null)
                {
                    return null;
                }

                result.Add(new JobSkill
                {
                    SkillId = dto.SkillId,
                    RequiredLevel = dto.RequiredLevel,
                    Skill = skill
                });
            }

            return result;
        }

        private static bool IsSalaryValid(
            decimal? minSalary,
            decimal? maxSalary
        )
        {
            if (
                minSalary.HasValue
                && maxSalary.HasValue
                && minSalary > maxSalary
            )
            {
                return false;
            }

            return true;
        }

        private static JobResponseDto Map(Job job)
        {
            return new JobResponseDto
            {
                Id = job.Id,
                CompanyId = job.CompanyId,
                CompanyName = job.Company.Name,
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                EmploymentType = job.EmploymentType,
                MinSalary = job.MinSalary,
                MaxSalary = job.MaxSalary,
                Status = job.Status,
                Deadline = job.Deadline,
                CreatedAt = job.CreatedAt,
                RequiredSkills = job.JobSkills
                    .Select(jobSkill =>
                        new JobSkillResponseDto
                        {
                            SkillId = jobSkill.SkillId,
                            SkillName =
                                jobSkill.Skill.Name,
                            Category =
                                jobSkill.Skill.Category,
                            RequiredLevel =
                                jobSkill.RequiredLevel
                        }
                    )
                    .ToList()
            };
        }
    }
}