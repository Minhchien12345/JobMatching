using System.ComponentModel.DataAnnotations;

namespace JobMatching.API.DTOs
{
    public class UpdateCandidateProfileDto
    {
        [Required]
        [MaxLength(150)]
        public string Headline { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Bio { get; set; } = string.Empty;

        [MaxLength(150)]
        public string Location { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Education { get; set; } = string.Empty;

        [Range(0, 100)]
        public int YearsOfExperience { get; set; }
    }
}