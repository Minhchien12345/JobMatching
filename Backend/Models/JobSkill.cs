using JobMatching.API.Enums;

namespace JobMatching.API.Models
{
    public class JobSkill
    {
        public int JobId { get; set; }

        public int SkillId { get; set; }

        public ProficiencyLevel RequiredLevel { get; set; }

        public Job Job { get; set; } = null!;

        public Skill Skill { get; set; } = null!;
    }
}