using System.ComponentModel.DataAnnotations;

namespace JobMatching.API.Models
{
    public class CandidateProfile
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [MaxLength(150)]
        public string Headline { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Bio { get; set; } = string.Empty;

        [MaxLength(150)]
        public string Location { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Education { get; set; } = string.Empty;

        public int YearsOfExperience { get; set; }

        public User User { get; set; } = null!;
    }
}