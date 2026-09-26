using JobMatching.API.DTOs;
using JobMatching.API.Enums;
using JobMatching.API.Models;
using JobMatching.API.Repositories;

namespace JobMatching.API.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(
            IAdminRepository adminRepository
        )
        {
            _adminRepository = adminRepository;
        }

        public async Task<AdminDashboardDto>
            GetDashboardAsync()
        {
            List<User> users =
                await _adminRepository.GetUsersAsync();

            List<Company> companies =
                await _adminRepository.GetCompaniesAsync();

            List<Job> jobs =
                await _adminRepository.GetJobsAsync();

            int applicationCount =
                await _adminRepository
                    .CountApplicationsAsync();

            return new AdminDashboardDto
            {
                TotalUsers = users.Count,

                TotalCandidates = users.Count(user =>
                    user.Role == UserRole.Candidate
                ),

                TotalRecruiters = users.Count(user =>
                    user.Role == UserRole.Recruiter
                ),

                TotalCompanies = companies.Count,

                ActiveCompanies = companies.Count(company =>
                    company.IsActive
                ),

                TotalJobs = jobs.Count,

                OpenJobs = jobs.Count(job =>
                    job.Status == JobStatus.Open
                ),

                TotalApplications = applicationCount
            };
        }

        public async Task<List<AdminUserResponseDto>>
            GetUsersAsync()
        {
            List<User> users =
                await _adminRepository.GetUsersAsync();

            return users.Select(MapUser).ToList();
        }

        public async Task<AdminUserResponseDto?>
            UpdateUserStatusAsync(
                int currentAdminId,
                int userId,
                bool isActive
            )
        {
            if (currentAdminId == userId)
            {
                return null;
            }

            User? user =
                await _adminRepository.GetUserByIdAsync(
                    userId
                );

            if (user == null)
            {
                return null;
            }

            user.IsActive = isActive;

            await _adminRepository.SaveChangesAsync();

            return MapUser(user);
        }

        public async Task<AdminUserResponseDto?>
            UpdateUserRoleAsync(
                int currentAdminId,
                int userId,
                UpdateUserRoleDto dto
            )
        {
            if (currentAdminId == userId)
            {
                return null;
            }

            User? user =
                await _adminRepository.GetUserByIdAsync(
                    userId
                );

            if (user == null)
            {
                return null;
            }

            user.Role = dto.Role;

            await _adminRepository.SaveChangesAsync();

            return MapUser(user);
        }

        public async Task<List<CompanyResponseDto>>
            GetCompaniesAsync()
        {
            List<Company> companies =
                await _adminRepository.GetCompaniesAsync();

            return companies.Select(MapCompany).ToList();
        }

        public async Task<CompanyResponseDto?>
            UpdateCompanyStatusAsync(
                int companyId,
                bool isActive
            )
        {
            Company? company =
                await _adminRepository.GetCompanyByIdAsync(
                    companyId
                );

            if (company == null)
            {
                return null;
            }

            company.IsActive = isActive;

            await _adminRepository.SaveChangesAsync();

            return MapCompany(company);
        }

        public async Task<List<JobResponseDto>>
            GetJobsAsync()
        {
            List<Job> jobs =
                await _adminRepository.GetJobsAsync();

            return jobs.Select(MapJob).ToList();
        }

        public async Task<JobResponseDto?> CloseJobAsync(
            int jobId
        )
        {
            Job? job =
                await _adminRepository.GetJobByIdAsync(jobId);

            if (job == null)
            {
                return null;
            }

            job.Status = JobStatus.Closed;

            await _adminRepository.SaveChangesAsync();

            return MapJob(job);
        }

        private static AdminUserResponseDto MapUser(
            User user
        )
        {
            return new AdminUserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }

        private static CompanyResponseDto MapCompany(
            Company company
        )
        {
            return new CompanyResponseDto
            {
                Id = company.Id,
                RecruiterId = company.RecruiterId,
                RecruiterName =
                    company.Recruiter.FullName,
                Name = company.Name,
                Description = company.Description,
                Industry = company.Industry,
                Location = company.Location,
                Website = company.Website,
                IsActive = company.IsActive,
                CreatedAt = company.CreatedAt
            };
        }

        private static JobResponseDto MapJob(Job job)
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