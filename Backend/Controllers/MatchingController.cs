using System.Security.Claims;
using JobMatching.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobMatching.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Candidate")]
    public class MatchingController : ControllerBase
    {
        private readonly IMatchingService _matchingService;

        public MatchingController(
            IMatchingService matchingService
        )
        {
            _matchingService = matchingService;
        }

        [HttpGet("jobs/{jobId:int}")]
        public async Task<IActionResult> GetJobMatch(
            int jobId
        )
        {
            var result =
                await _matchingService.CalculateAsync(
                    GetCurrentUserId(),
                    jobId
                );

            if (result == null)
            {
                return NotFound(new
                {
                    message =
                        "Job is unavailable or not found."
                });
            }

            return Ok(result);
        }

        [HttpGet("recommendations")]
        public async Task<IActionResult> GetRecommendations()
        {
            return Ok(
                await _matchingService
                    .GetRecommendationsAsync(
                        GetCurrentUserId()
                    )
            );
        }

        private int GetCurrentUserId()
        {
            return int.Parse(
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier
                )!
            );
        }
    }
}