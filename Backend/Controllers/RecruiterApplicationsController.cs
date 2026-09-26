using System.Security.Claims;
using JobMatching.API.DTOs;
using JobMatching.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobMatching.API.Controllers
{
    [ApiController]
    [Route("api/recruiter")]
    [Authorize(Roles = "Recruiter")]
    public class RecruiterApplicationsController
        : ControllerBase
    {
        private readonly IApplicationService
            _applicationService;

        public RecruiterApplicationsController(
            IApplicationService applicationService
        )
        {
            _applicationService = applicationService;
        }

        [HttpGet("jobs/{jobId:int}/applications")]
        public async Task<IActionResult> GetByJob(
            int jobId
        )
        {
            return Ok(
                await _applicationService
                    .GetJobApplicationsAsync(
                        GetCurrentUserId(),
                        jobId
                    )
            );
        }

        [HttpPatch(
            "applications/{applicationId:int}/status"
        )]
        public async Task<IActionResult> UpdateStatus(
            int applicationId,
            UpdateApplicationStatusDto dto
        )
        {
            var result =
                await _applicationService.UpdateStatusAsync(
                    GetCurrentUserId(),
                    applicationId,
                    dto
                );

            if (result == null)
            {
                return BadRequest(new
                {
                    message =
                        "Application not found, access denied or status invalid."
                });
            }

            return Ok(result);
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