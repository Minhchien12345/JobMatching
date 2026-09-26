namespace JobMatching.API.DTOs
{
    public class MatchResultDto
    {
        public int JobId { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public int CompanyId { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public int RequiredSkillCount { get; set; }

        public int MatchedSkillCount { get; set; }

        public decimal MatchScore { get; set; }

        public List<SkillMatchDetailDto> MatchedSkills
        {
            get;
            set;
        } = new();

        public List<SkillMatchDetailDto> MissingSkills
        {
            get;
            set;
        } = new();
    }
}