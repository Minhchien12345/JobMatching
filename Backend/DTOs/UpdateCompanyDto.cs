using System.ComponentModel.DataAnnotations;

namespace JobMatching.API.DTOs
{
    public class UpdateCompanyDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Industry { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Location { get; set; } = string.Empty;

        [Url]
        [MaxLength(500)]
        public string Website { get; set; } = string.Empty;
    }
}