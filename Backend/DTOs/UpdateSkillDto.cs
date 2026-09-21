using System.ComponentModel.DataAnnotations;

namespace JobMatching.API.DTOs
{
    public class UpdateSkillDto
    {
        [Required(ErrorMessage = "Skill name is required.")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required.")]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;
    }
}