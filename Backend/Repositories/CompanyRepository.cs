using JobMatching.API.Data;
using JobMatching.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JobMatching.API.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Company>> GetAllActiveAsync()
        {
            return await _context.Companies
                .Include(company => company.Recruiter)
                .Where(company => company.IsActive)
                .OrderBy(company => company.Name)
                .ToListAsync();
        }

        public async Task<Company?> GetByIdAsync(int id)
        {
            return await _context.Companies
                .Include(company => company.Recruiter)
                .FirstOrDefaultAsync(company =>
                    company.Id == id
                );
        }

        public async Task<Company?> GetByRecruiterIdAsync(
            int recruiterId
        )
        {
            return await _context.Companies
                .Include(company => company.Recruiter)
                .FirstOrDefaultAsync(company =>
                    company.RecruiterId == recruiterId
                );
        }

        public async Task<Company> CreateAsync(
            Company company
        )
        {
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();

            return company;
        }

        public async Task UpdateAsync(Company company)
        {
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
        }
    }
}