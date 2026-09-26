using JobMatching.API.Data;
using JobMatching.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JobMatching.API.Repositories
{
    public class ApplicationRepository
        : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        private IQueryable<JobApplication> Query()
        {
            return _context.JobApplications
                .Include(application => application.Candidate)
                .Include(application => application.Job)
                    .ThenInclude(job => job.Company);
        }

        public async Task<JobApplication?> GetByIdAsync(
            int id
        )
        {
            return await Query().FirstOrDefaultAsync(
                application => application.Id == id
            );
        }

        public async Task<JobApplication?>
            GetByJobAndCandidateAsync(
                int jobId,
                int candidateId
            )
        {
            return await Query().FirstOrDefaultAsync(
                application =>
                    application.JobId == jobId
                    && application.CandidateId
                        == candidateId
            );
        }

        public async Task<List<JobApplication>>
            GetByCandidateIdAsync(int candidateId)
        {
            return await Query()
                .Where(application =>
                    application.CandidateId == candidateId
                )
                .OrderByDescending(application =>
                    application.AppliedAt
                )
                .ToListAsync();
        }

        public async Task<List<JobApplication>>
            GetByJobIdAsync(int jobId)
        {
            return await Query()
                .Where(application =>
                    application.JobId == jobId
                )
                .OrderByDescending(application =>
                    application.AppliedAt
                )
                .ToListAsync();
        }

        public async Task<JobApplication> CreateAsync(
            JobApplication application
        )
        {
            await _context.JobApplications.AddAsync(
                application
            );

            await _context.SaveChangesAsync();

            return application;
        }

        public async Task UpdateAsync(
            JobApplication application
        )
        {
            _context.JobApplications.Update(application);
            await _context.SaveChangesAsync();
        }
    }
}