using JobMatching.API.Enums;

namespace JobMatching.API.Models
{
    public class CandidateSkill
    {
        public int UserId { get; set; }

        public int SkillId { get; set; }

        public ProficiencyLevel ProficiencyLevel { get; set; }

        public decimal YearsOfExperience { get; set; }

        public User User { get; set; } = null!;

        public Skill Skill { get; set; } = null!;
    }
}