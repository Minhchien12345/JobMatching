using JobMatching.API.DTOs;
using JobMatching.API.Models;

namespace JobMatching.API.Services
{
    public interface ISkillService
    {
        Task<List<Skill>> GetAllAsync();

        Task<Skill?> GetByIdAsync(int id);

        Task<Skill> CreateAsync(CreateSkillDto dto);

        Task<Skill?> UpdateAsync(int id, UpdateSkillDto dto);

        Task<bool> DeleteAsync(int id);
    }
}