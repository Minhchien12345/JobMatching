using JobMatching.API.DTOs;
using JobMatching.API.Enums;
using JobMatching.API.Models;
using JobMatching.API.Repositories;

namespace JobMatching.API.Services
{
    public class ApplicationService
        : IApplicationService
    {
        private readonly IApplicationRepository
            _applicationRepository;

        private readonly IJobRepository _jobRepository;

        public ApplicationService(
            IApplicationRepository applicationRepository,
            IJobRepository jobRepository
        )
        {
            _applicationRepository = applicationRepository;
            _jobRepository = jobRepository;
        }

        public async Task<ApplicationResponseDto?>
            ApplyAsync(
                int candidateId,
                int jobId,
                ApplyJobDto dto
            )
        {
            Job? job =
                await _jobRepository.GetByIdAsync(jobId);

            if (
                job == null
                || job.Status != JobStatus.Open
                || !job.Company.IsActive
                || (
                    job.Deadline.HasValue
                    && job.Deadline < DateTime.UtcNow
                )
            )
            {
                return null;
            }

            JobApplication? existing =
                await _applicationRepository
                    .GetByJobAndCandidateAsync(
                        jobId,
                        candidateId
                    );

            if (existing != null)
            {
                return null;
            }

            var application = new JobApplication
            {
                JobId = jobId,
                CandidateId = candidateId,
                CoverLetter = dto.CoverLetter.Trim(),
                Status = ApplicationStatus.Applied,
                AppliedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _applicationRepository.CreateAsync(
                application
            );

            JobApplication? created =
                await _applicationRepository.GetByIdAsync(
                    application.Id
                );

            return Map(created!);
        }

        public async Task<List<ApplicationResponseDto>>
            GetMyApplicationsAsync(int candidateId)
        {
            List<JobApplication> applications =
                await _applicationRepository
                    .GetByCandidateIdAsync(candidateId);

            return applications.Select(Map).ToList();
        }

        public async Task<ApplicationResponseDto?>
            GetMyApplicationByIdAsync(
                int candidateId,
                int applicationId
            )
        {
            JobApplication? application =
                await _applicationRepository
                    .GetByIdAsync(applicationId);

            if (
                application == null
                || application.CandidateId != candidateId
            )
            {
                return null;
            }

            return Map(application);
        }

        public async Task<bool> WithdrawAsync(
            int candidateId,
            int applicationId
        )
        {
            JobApplication? application =
                await _applicationRepository
                    .GetByIdAsync(applicationId);

            if (
                application == null
                || application.CandidateId != candidateId
            )
            {
                return false;
            }

            if (
                application.Status
                    == ApplicationStatus.Accepted
                || application.Status
                    == ApplicationStatus.Rejected
                || application.Status
                    == ApplicationStatus.Withdrawn
            )
            {
                return false;
            }

            application.Status =
                ApplicationStatus.Withdrawn;

            application.UpdatedAt = DateTime.UtcNow;

            await _applicationRepository.UpdateAsync(
                application
            );

            return true;
        }

        public async Task<List<ApplicationResponseDto>>
            GetJobApplicationsAsync(
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
                return new List<ApplicationResponseDto>();
            }

            List<JobApplication> applications =
                await _applicationRepository
                    .GetByJobIdAsync(jobId);

            return applications.Select(Map).ToList();
        }

        public async Task<ApplicationResponseDto?>
            UpdateStatusAsync(
                int recruiterId,
                int applicationId,
                UpdateApplicationStatusDto dto
            )
        {
            JobApplication? application =
                await _applicationRepository
                    .GetByIdAsync(applicationId);

            if (
                application == null
                || application.Job.Company.RecruiterId
                    != recruiterId
            )
            {
                return null;
            }

            bool validStatus =
                dto.Status == ApplicationStatus.Reviewing
                || dto.Status
                    == ApplicationStatus.Interviewing
                || dto.Status == ApplicationStatus.Accepted
                || dto.Status == ApplicationStatus.Rejected;

            if (!validStatus)
            {
                return null;
            }

            if (
                application.Status
                    == ApplicationStatus.Withdrawn
            )
            {
                return null;
            }

            application.Status = dto.Status;
            application.UpdatedAt = DateTime.UtcNow;

            await _applicationRepository.UpdateAsync(
                application
            );

            return Map(application);
        }

        private static ApplicationResponseDto Map(
            JobApplication application
        )
        {
            return new ApplicationResponseDto
            {
                Id = application.Id,
                JobId = application.JobId,
                JobTitle = application.Job.Title,
                CompanyId =
                    application.Job.CompanyId,
                CompanyName =
                    application.Job.Company.Name,
                CandidateId =
                    application.CandidateId,
                CandidateName =
                    application.Candidate.FullName,
                CandidateEmail =
                    application.Candidate.Email,
                CoverLetter =
                    application.CoverLetter,
                Status = application.Status,
                AppliedAt = application.AppliedAt,
                UpdatedAt = application.UpdatedAt
            };
        }
    }
}