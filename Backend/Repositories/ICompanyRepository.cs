using JobMatching.API.Models;

namespace JobMatching.API.Repositories
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetAllActiveAsync();

        Task<Company?> GetByIdAsync(int id);

        Task<Company?> GetByRecruiterIdAsync(
            int recruiterId
        );

        Task<Company> CreateAsync(Company company);

        Task UpdateAsync(Company company);
    }
}