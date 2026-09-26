using JobMatching.API.DTOs;

namespace JobMatching.API.Services
{
    public interface IAdminService
    {
        Task<AdminDashboardDto> GetDashboardAsync();

        Task<List<AdminUserResponseDto>> GetUsersAsync();

        Task<AdminUserResponseDto?> UpdateUserStatusAsync(
            int currentAdminId,
            int userId,
            bool isActive
        );

        Task<AdminUserResponseDto?> UpdateUserRoleAsync(
            int currentAdminId,
            int userId,
            UpdateUserRoleDto dto
        );

        Task<List<CompanyResponseDto>> GetCompaniesAsync();

        Task<CompanyResponseDto?> UpdateCompanyStatusAsync(
            int companyId,
            bool isActive
        );

        Task<List<JobResponseDto>> GetJobsAsync();

        Task<JobResponseDto?> CloseJobAsync(int jobId);
    }
}