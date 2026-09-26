using JobMatching.API.Data;
using JobMatching.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JobMatching.API.Repositories
{
    public class CandidateSkillRepository
        : ICandidateSkillRepository
    {
        private readonly AppDbContext _context;

        public CandidateSkillRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CandidateSkill>> GetAllAsync(
            int userId
        )
        {
            return await _context.CandidateSkills
                .Include(candidateSkill =>
                    candidateSkill.Skill
                )
                .Where(candidateSkill =>
                    candidateSkill.UserId == userId
                )
                .ToListAsync();
        }

        public async Task<CandidateSkill?> GetAsync(
            int userId,
            int skillId
        )
        {
            return await _context.CandidateSkills
                .Include(candidateSkill =>
                    candidateSkill.Skill
                )
                .FirstOrDefaultAsync(candidateSkill =>
                    candidateSkill.UserId == userId
                    && candidateSkill.SkillId == skillId
                );
        }

        public async Task<CandidateSkill> CreateAsync(
            CandidateSkill candidateSkill
        )
        {
            await _context.CandidateSkills.AddAsync(
                candidateSkill
            );

            await _context.SaveChangesAsync();

            return candidateSkill;
        }

        public async Task UpdateAsync(
            CandidateSkill candidateSkill
        )
        {
            _context.CandidateSkills.Update(candidateSkill);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(
            CandidateSkill candidateSkill
        )
        {
            _context.CandidateSkills.Remove(candidateSkill);
            await _context.SaveChangesAsync();
        }
    }
}