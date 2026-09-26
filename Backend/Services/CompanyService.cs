using JobMatching.API.DTOs;
using JobMatching.API.Models;
using JobMatching.API.Repositories;

namespace JobMatching.API.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(
            ICompanyRepository companyRepository
        )
        {
            _companyRepository = companyRepository;
        }

        public async Task<List<CompanyResponseDto>>
            GetAllActiveAsync()
        {
            List<Company> companies =
                await _companyRepository.GetAllActiveAsync();

            return companies.Select(Map).ToList();
        }

        public async Task<CompanyResponseDto?> GetByIdAsync(
            int id
        )
        {
            Company? company =
                await _companyRepository.GetByIdAsync(id);

            if (company == null || !company.IsActive)
            {
                return null;
            }

            return Map(company);
        }

        public async Task<CompanyResponseDto?> GetMineAsync(
            int recruiterId
        )
        {
            Company? company =
                await _companyRepository
                    .GetByRecruiterIdAsync(recruiterId);

            return company == null ? null : Map(company);
        }

        public async Task<CompanyResponseDto?> CreateAsync(
            int recruiterId,
            CreateCompanyDto dto
        )
        {
            Company? existing =
                await _companyRepository
                    .GetByRecruiterIdAsync(recruiterId);

            if (existing != null)
            {
                return null;
            }

            var company = new Company
            {
                RecruiterId = recruiterId,
                Name = dto.Name.Trim(),
                Description = dto.Description.Trim(),
                Industry = dto.Industry.Trim(),
                Location = dto.Location.Trim(),
                Website = dto.Website.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _companyRepository.CreateAsync(company);

            company = await _companyRepository
                .GetByIdAsync(company.Id);

            return Map(company!);
        }

        public async Task<CompanyResponseDto?> UpdateAsync(
            int recruiterId,
            UpdateCompanyDto dto
        )
        {
            Company? company =
                await _companyRepository
                    .GetByRecruiterIdAsync(recruiterId);

            if (company == null)
            {
                return null;
            }

            company.Name = dto.Name.Trim();
            company.Description = dto.Description.Trim();
            company.Industry = dto.Industry.Trim();
            company.Location = dto.Location.Trim();
            company.Website = dto.Website.Trim();

            await _companyRepository.UpdateAsync(company);

            return Map(company);
        }

        public async Task<CompanyResponseDto?>
            UpdateStatusAsync(
                int recruiterId,
                bool isActive
            )
        {
            Company? company =
                await _companyRepository
                    .GetByRecruiterIdAsync(recruiterId);

            if (company == null)
            {
                return null;
            }

            company.IsActive = isActive;

            await _companyRepository.UpdateAsync(company);

            return Map(company);
        }

        private static CompanyResponseDto Map(
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
    }
}