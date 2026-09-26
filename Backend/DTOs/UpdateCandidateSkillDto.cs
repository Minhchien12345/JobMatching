using System.ComponentModel.DataAnnotations;
using JobMatching.API.Enums;

namespace JobMatching.API.DTOs
{
    public class UpdateCandidateSkillDto
    {
        public ProficiencyLevel ProficiencyLevel { get; set; }

        [Range(0, 100)]
        public decimal YearsOfExperience { get; set; }
    }
}