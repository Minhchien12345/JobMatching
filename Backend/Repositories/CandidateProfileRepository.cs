using JobMatching.API.Data;
using JobMatching.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JobMatching.API.Repositories
{
    public class CandidateProfileRepository
        : ICandidateProfileRepository
    {
        private readonly AppDbContext _context;

        public CandidateProfileRepository(
            AppDbContext context
        )
        {
            _context = context;
        }

        public async Task<CandidateProfile?>
            GetByUserIdAsync(int userId)
        {
            return await _context.CandidateProfiles
                .Include(profile => profile.User)
                .FirstOrDefaultAsync(
                    profile => profile.UserId == userId
                );
        }

        public async Task<CandidateProfile> CreateAsync(
            CandidateProfile profile
        )
        {
            await _context.CandidateProfiles.AddAsync(profile);
            await _context.SaveChangesAsync();

            return profile;
        }

        public async Task UpdateAsync(
            CandidateProfile profile
        )
        {
            _context.CandidateProfiles.Update(profile);
            await _context.SaveChangesAsync();
        }
    }
}