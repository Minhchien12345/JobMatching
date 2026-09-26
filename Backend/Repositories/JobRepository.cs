using JobMatching.API.Data;
using JobMatching.API.Enums;
using JobMatching.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JobMatching.API.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDbContext _context;

        public JobRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Job>> SearchOpenAsync(
            string? keyword,
            string? location
        )
        {
            IQueryable<Job> query = _context.Jobs
                .Include(job => job.Company)
                .Include(job => job.JobSkills)
                    .ThenInclude(jobSkill => jobSkill.Skill)
                .Where(job =>
                    job.Status == JobStatus.Open
                    && job.Company.IsActive
                    && (
                        job.Deadline == null
                        || job.Deadline >= DateTime.UtcNow
                    )
                );

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string search = keyword.Trim();

                query = query.Where(job =>
                    job.Title.Contains(search)
                    || job.Description.Contains(search)
                    || job.Company.Name.Contains(search)
                );
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                string searchLocation = location.Trim();

                query = query.Where(job =>
                    job.Location.Contains(searchLocation)
                );
            }

            return await query
                .OrderByDescending(job => job.CreatedAt)
                .ToListAsync();
        }

        public async Task<Job?> GetByIdAsync(int id)
        {
            return await _context.Jobs
                .Include(job => job.Company)
                .Include(job => job.JobSkills)
                    .ThenInclude(jobSkill => jobSkill.Skill)
                .FirstOrDefaultAsync(job => job.Id == id);
        }

        public async Task<List<Job>> GetByRecruiterIdAsync(
            int recruiterId
        )
        {
            return await _context.Jobs
                .Include(job => job.Company)
                .Include(job => job.JobSkills)
                    .ThenInclude(jobSkill => jobSkill.Skill)
                .Where(job =>
                    job.Company.RecruiterId == recruiterId
                )
                .OrderByDescending(job => job.CreatedAt)
                .ToListAsync();
        }

        public async Task<Job> CreateAsync(Job job)
        {
            await _context.Jobs.AddAsync(job);
            await _context.SaveChangesAsync();

            return job;
        }

        public async Task UpdateAsync(Job job)
        {
            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();
        }
    }
}