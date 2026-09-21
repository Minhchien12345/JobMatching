using JobMatching.API.DTOs;
using JobMatching.API.Models;
using JobMatching.API.Repositories;

namespace JobMatching.API.Services
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;

        public SkillService(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<List<Skill>> GetAllAsync()
        {
            return await _skillRepository.GetAllAsync();
        }

        public async Task<Skill?> GetByIdAsync(int id)
        {
            return await _skillRepository.GetByIdAsync(id);
        }

        public async Task<Skill> CreateAsync(CreateSkillDto dto)
        {
            var skill = new Skill
            {
                Name = dto.Name.Trim(),
                Category = dto.Category.Trim()
            };

            return await _skillRepository.CreateAsync(skill);
        }

        public async Task<Skill?> UpdateAsync(
            int id,
            UpdateSkillDto dto
        )
        {
            var skill = await _skillRepository.GetByIdAsync(id);

            if (skill == null)
            {
                return null;
            }

            skill.Name = dto.Name.Trim();
            skill.Category = dto.Category.Trim();

            await _skillRepository.UpdateAsync(skill);

            return skill;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var skill = await _skillRepository.GetByIdAsync(id);

            if (skill == null)
            {
                return false;
            }

            await _skillRepository.DeleteAsync(skill);

            return true;
        }
    }
}