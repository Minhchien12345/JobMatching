using JobMatching.API.Models;

namespace JobMatching.API.Repositories
{
    public interface IAdminRepository
    {
        Task<List<User>> GetUsersAsync();

        Task<User?> GetUserByIdAsync(int userId);

        Task<List<Company>> GetCompaniesAsync();

        Task<Company?> GetCompanyByIdAsync(int companyId);

        Task<List<Job>> GetJobsAsync();

        Task<Job?> GetJobByIdAsync(int jobId);

        Task<int> CountApplicationsAsync();

        Task SaveChangesAsync();
    }
}