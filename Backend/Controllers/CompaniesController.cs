using JobMatching.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobMatching.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(
            ICompanyService companyService
        )
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(
                await _companyService.GetAllActiveAsync()
            );
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company =
                await _companyService.GetByIdAsync(id);

            if (company == null)
            {
                return NotFound(new
                {
                    message = "Company not found."
                });
            }

            return Ok(company);
        }
    }
}