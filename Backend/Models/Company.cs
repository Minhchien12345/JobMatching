using System.ComponentModel.DataAnnotations;

namespace JobMatching.API.Models
{
    public class Company
    {
        public int Id { get; set; }

        public int RecruiterId { get; set; }

        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(150)]
        public string Industry { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Location { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Website { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User Recruiter { get; set; } = null!;
    }
}