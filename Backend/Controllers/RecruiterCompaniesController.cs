using System.Security.Claims;
using JobMatching.API.DTOs;
using JobMatching.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobMatching.API.Controllers
{
    [ApiController]
    [Route("api/recruiter/company")]
    [Authorize(Roles = "Recruiter")]
    public class RecruiterCompaniesController
        : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public RecruiterCompaniesController(
            ICompanyService companyService
        )
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMine()
        {
            var company =
                await _companyService.GetMineAsync(
                    GetCurrentUserId()
                );

            if (company == null)
            {
                return NotFound(new
                {
                    message = "Company not found."
                });
            }

            return Ok(company);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateCompanyDto dto
        )
        {
            var company =
                await _companyService.CreateAsync(
                    GetCurrentUserId(),
                    dto
                );

            if (company == null)
            {
                return Conflict(new
                {
                    message =
                        "Recruiter already has a company."
                });
            }

            return StatusCode(
                StatusCodes.Status201Created,
                company
            );
        }

        [HttpPut]
        public async Task<IActionResult> Update(
            UpdateCompanyDto dto
        )
        {
            var company =
                await _companyService.UpdateAsync(
                    GetCurrentUserId(),
                    dto
                );

            if (company == null)
            {
                return NotFound(new
                {
                    message = "Company not found."
                });
            }

            return Ok(company);
        }

        [HttpPatch("status")]
        public async Task<IActionResult> UpdateStatus(
            UpdateCompanyStatusDto dto
        )
        {
            var company =
                await _companyService.UpdateStatusAsync(
                    GetCurrentUserId(),
                    dto.IsActive
                );

            if (company == null)
            {
                return NotFound(new
                {
                    message = "Company not found."
                });
            }

            return Ok(company);
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