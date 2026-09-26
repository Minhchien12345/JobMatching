using JobMatching.API.Data;
using JobMatching.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JobMatching.API.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users
                .OrderByDescending(user => user.CreatedAt)
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(
            int userId
        )
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<List<Company>> GetCompaniesAsync()
        {
            return await _context.Companies
                .Include(company => company.Recruiter)
                .OrderBy(company => company.Name)
                .ToListAsync();
        }

        public async Task<Company?> GetCompanyByIdAsync(
            int companyId
        )
        {
            return await _context.Companies
                .Include(company => company.Recruiter)
                .FirstOrDefaultAsync(company =>
                    company.Id == companyId
                );
        }

        public async Task<List<Job>> GetJobsAsync()
        {
            return await _context.Jobs
                .Include(job => job.Company)
                .Include(job => job.JobSkills)
                    .ThenInclude(jobSkill => jobSkill.Skill)
                .OrderByDescending(job => job.CreatedAt)
                .ToListAsync();
        }

        public async Task<Job?> GetJobByIdAsync(int jobId)
        {
            return await _context.Jobs
                .Include(job => job.Company)
                .Include(job => job.JobSkills)
                    .ThenInclude(jobSkill => jobSkill.Skill)
                .FirstOrDefaultAsync(job => job.Id == jobId);
        }

        public async Task<int> CountApplicationsAsync()
        {
            return await _context.JobApplications.CountAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}