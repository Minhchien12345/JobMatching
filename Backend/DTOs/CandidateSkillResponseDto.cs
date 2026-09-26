using JobMatching.API.Enums;

namespace JobMatching.API.DTOs
{
    public class CandidateSkillResponseDto
    {
        public int SkillId { get; set; }

        public string SkillName { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public ProficiencyLevel ProficiencyLevel { get; set; }

        public decimal YearsOfExperience { get; set; }
    }
}