using System.ComponentModel.DataAnnotations;
using JobMatching.API.Enums;

namespace JobMatching.API.DTOs
{
    public class AddCandidateSkillDto
    {
        [Range(1, int.MaxValue)]
        public int SkillId { get; set; }

        public ProficiencyLevel ProficiencyLevel { get; set; }

        [Range(0, 100)]
        public decimal YearsOfExperience { get; set; }
    }
}