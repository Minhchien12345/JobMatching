using JobMatching.API.DTOs;
using JobMatching.API.Models;
using JobMatching.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobMatching.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillsController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Skill>>> GetAll()
        {
            var skills = await _skillService.GetAllAsync();

            return Ok(skills);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Skill>> GetById(int id)
        {
            var skill = await _skillService.GetByIdAsync(id);

            if (skill == null)
            {
                return NotFound(new
                {
                    message = $"Skill with ID {id} was not found."
                });
            }

            return Ok(skill);
        }

        [HttpPost]
        public async Task<ActionResult<Skill>> Create(CreateSkillDto dto)
        {
            var createdSkill = await _skillService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdSkill.Id },
                createdSkill
            );
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Skill>> Update(
            int id,
            UpdateSkillDto dto
        )
        {
            var updatedSkill =
                await _skillService.UpdateAsync(id, dto);

            if (updatedSkill == null)
            {
                return NotFound(new
                {
                    message = $"Skill with ID {id} was not found."
                });
            }

            return Ok(updatedSkill);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _skillService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = $"Skill with ID {id} was not found."
                });
            }

            return NoContent();
        }
    }
}