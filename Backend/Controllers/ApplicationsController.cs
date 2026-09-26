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
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService
            _applicationService;

        public ApplicationsController(
            IApplicationService applicationService
        )
        {
            _applicationService = applicationService;
        }

        [HttpPost("jobs/{jobId:int}")]
        public async Task<IActionResult> Apply(
            int jobId,
            ApplyJobDto dto
        )
        {
            var result =
                await _applicationService.ApplyAsync(
                    GetCurrentUserId(),
                    jobId,
                    dto
                );

            if (result == null)
            {
                return BadRequest(new
                {
                    message =
                        "Job is unavailable or you already applied."
                });
            }

            return StatusCode(
                StatusCodes.Status201Created,
                result
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetMine()
        {
            return Ok(
                await _applicationService
                    .GetMyApplicationsAsync(
                        GetCurrentUserId()
                    )
            );
        }

        [HttpGet("{applicationId:int}")]
        public async Task<IActionResult> GetById(
            int applicationId
        )
        {
            var result =
                await _applicationService
                    .GetMyApplicationByIdAsync(
                        GetCurrentUserId(),
                        applicationId
                    );

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPatch("{applicationId:int}/withdraw")]
        public async Task<IActionResult> Withdraw(
            int applicationId
        )
        {
            bool withdrawn =
                await _applicationService.WithdrawAsync(
                    GetCurrentUserId(),
                    applicationId
                );

            if (!withdrawn)
            {
                return BadRequest(new
                {
                    message =
                        "Application cannot be withdrawn."
                });
            }

            return Ok(new
            {
                message =
                    "Application withdrawn successfully."
            });
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