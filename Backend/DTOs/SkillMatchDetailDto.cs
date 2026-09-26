using JobMatching.API.Enums;

namespace JobMatching.API.DTOs
{
    public class SkillMatchDetailDto
    {
        public int SkillId { get; set; }

        public string SkillName { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public ProficiencyLevel RequiredLevel { get; set; }

        public ProficiencyLevel? CandidateLevel { get; set; }

        public bool MeetsRequiredLevel { get; set; }
    }
}