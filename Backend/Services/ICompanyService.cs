using JobMatching.API.DTOs;

namespace JobMatching.API.Services
{
    public interface ICompanyService
    {
        Task<List<CompanyResponseDto>> GetAllActiveAsync();

        Task<CompanyResponseDto?> GetByIdAsync(int id);

        Task<CompanyResponseDto?> GetMineAsync(
            int recruiterId
        );

        Task<CompanyResponseDto?> CreateAsync(
            int recruiterId,
            CreateCompanyDto dto
        );

        Task<CompanyResponseDto?> UpdateAsync(
            int recruiterId,
            UpdateCompanyDto dto
        );

        Task<CompanyResponseDto?> UpdateStatusAsync(
            int recruiterId,
            bool isActive
        );
    }
}