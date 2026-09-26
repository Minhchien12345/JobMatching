using System.ComponentModel.DataAnnotations;
using JobMatching.API.Enums;

namespace JobMatching.API.DTOs
{
    public class JobSkillInputDto
    {
        [Range(1, int.MaxValue)]
        public int SkillId { get; set; }

        public ProficiencyLevel RequiredLevel { get; set; }
    }
}