using System.Security.Claims;
using JobMatching.API.DTOs;
using JobMatching.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobMatching.API.Controllers
{
    [ApiController]
    [Route("api/recruiter/jobs")]
    [Authorize(Roles = "Recruiter")]
    public class RecruiterJobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public RecruiterJobsController(
            IJobService jobService
        )
        {
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMine()
        {
            return Ok(
                await _jobService.GetMineAsync(
                    GetCurrentUserId()
                )
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateJobDto dto
        )
        {
            var job =
                await _jobService.CreateAsync(
                    GetCurrentUserId(),
                    dto
                );

            if (job == null)
            {
                return BadRequest(new
                {
                    message =
                        "Invalid company, salary or required skills."
                });
            }

            return StatusCode(
                StatusCodes.Status201Created,
                job
            );
        }

        [HttpPut("{jobId:int}")]
        public async Task<IActionResult> Update(
            int jobId,
            UpdateJobDto dto
        )
        {
            var job =
                await _jobService.UpdateAsync(
                    GetCurrentUserId(),
                    jobId,
                    dto
                );

            if (job == null)
            {
                return BadRequest(new
                {
                    message =
                        "Job not found, access denied or data is invalid."
                });
            }

            return Ok(job);
        }

        [HttpPatch("{jobId:int}/close")]
        public async Task<IActionResult> Close(int jobId)
        {
            bool closed =
                await _jobService.CloseAsync(
                    GetCurrentUserId(),
                    jobId
                );

            if (!closed)
            {
                return NotFound(new
                {
                    message = "Job not found."
                });
            }

            return Ok(new
            {
                message = "Job closed successfully."
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