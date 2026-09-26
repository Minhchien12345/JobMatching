using JobMatching.API.Enums;

namespace JobMatching.API.DTOs
{
    public class JobResponseDto
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public EmploymentType EmploymentType { get; set; }

        public decimal? MinSalary { get; set; }

        public decimal? MaxSalary { get; set; }

        public JobStatus Status { get; set; }

        public DateTime? Deadline { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<JobSkillResponseDto> RequiredSkills { get; set; }
            = new();
    }
}