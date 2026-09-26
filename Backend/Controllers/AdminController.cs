using System.Security.Claims;
using JobMatching.API.DTOs;
using JobMatching.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobMatching.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            return Ok(
                await _adminService.GetDashboardAsync()
            );
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(
                await _adminService.GetUsersAsync()
            );
        }

        [HttpPatch("users/{userId:int}/status")]
        public async Task<IActionResult> UpdateUserStatus(
            int userId,
            UpdateUserStatusDto dto
        )
        {
            var user =
                await _adminService.UpdateUserStatusAsync(
                    GetCurrentUserId(),
                    userId,
                    dto.IsActive
                );

            if (user == null)
            {
                return BadRequest(new
                {
                    message =
                        "User not found or you cannot update your own status."
                });
            }

            return Ok(user);
        }

        [HttpPatch("users/{userId:int}/role")]
        public async Task<IActionResult> UpdateUserRole(
            int userId,
            UpdateUserRoleDto dto
        )
        {
            var user =
                await _adminService.UpdateUserRoleAsync(
                    GetCurrentUserId(),
                    userId,
                    dto
                );

            if (user == null)
            {
                return BadRequest(new
                {
                    message =
                        "User not found or you cannot change your own role."
                });
            }

            return Ok(user);
        }

        [HttpGet("companies")]
        public async Task<IActionResult> GetCompanies()
        {
            return Ok(
                await _adminService.GetCompaniesAsync()
            );
        }

        [HttpPatch("companies/{companyId:int}/status")]
        public async Task<IActionResult> UpdateCompanyStatus(
            int companyId,
            UpdateCompanyStatusDto dto
        )
        {
            var company =
                await _adminService
                    .UpdateCompanyStatusAsync(
                        companyId,
                        dto.IsActive
                    );

            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        [HttpGet("jobs")]
        public async Task<IActionResult> GetJobs()
        {
            return Ok(
                await _adminService.GetJobsAsync()
            );
        }

        [HttpPatch("jobs/{jobId:int}/close")]
        public async Task<IActionResult> CloseJob(int jobId)
        {
            var job =
                await _adminService.CloseJobAsync(jobId);

            if (job == null)
            {
                return NotFound();
            }

            return Ok(job);
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