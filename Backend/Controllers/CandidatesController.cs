using System.Security.Claims;
using JobMatching.API.DTOs;
using JobMatching.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobMatching.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Candidate")]
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidateService _candidateService;

        public CandidatesController(
            ICandidateService candidateService
        )
        {
            _candidateService = candidateService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            int userId = GetCurrentUserId();

            var profile =
                await _candidateService.GetProfileAsync(
                    userId
                );

            if (profile == null)
            {
                return NotFound(new
                {
                    message = "Candidate profile not found."
                });
            }

            return Ok(profile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(
            UpdateCandidateProfileDto dto
        )
        {
            int userId = GetCurrentUserId();

            var profile =
                await _candidateService.UpdateProfileAsync(
                    userId,
                    dto
                );

            return Ok(profile);
        }

        [HttpGet("skills")]
        public async Task<IActionResult> GetSkills()
        {
            int userId = GetCurrentUserId();

            return Ok(
                await _candidateService.GetSkillsAsync(userId)
            );
        }

        [HttpPost("skills")]
        public async Task<IActionResult> AddSkill(
            AddCandidateSkillDto dto
        )
        {
            int userId = GetCurrentUserId();

            var result =
                await _candidateService.AddSkillAsync(
                    userId,
                    dto
                );

            if (result == null)
            {
                return BadRequest(new
                {
                    message =
                        "Skill does not exist or was already added."
                });
            }

            return StatusCode(
                StatusCodes.Status201Created,
                result
            );
        }

        [HttpPut("skills/{skillId:int}")]
        public async Task<IActionResult> UpdateSkill(
            int skillId,
            UpdateCandidateSkillDto dto
        )
        {
            int userId = GetCurrentUserId();

            var result =
                await _candidateService.UpdateSkillAsync(
                    userId,
                    skillId,
                    dto
                );

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Candidate skill not found."
                });
            }

            return Ok(result);
        }

        [HttpDelete("skills/{skillId:int}")]
        public async Task<IActionResult> DeleteSkill(
            int skillId
        )
        {
            int userId = GetCurrentUserId();

            bool deleted =
                await _candidateService.DeleteSkillAsync(
                    userId,
                    skillId
                );

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Candidate skill not found."
                });
            }

            return NoContent();
        }

        private int GetCurrentUserId()
        {
            string? userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier
                );

            return int.Parse(userId!);
        }
    }
}