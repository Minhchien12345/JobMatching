using System.ComponentModel.DataAnnotations;
using JobMatching.API.Enums;

namespace JobMatching.API.Models
{
    public class JobApplication
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        public int CandidateId { get; set; }

        [MaxLength(2000)]
        public string CoverLetter { get; set; } = string.Empty;

        public ApplicationStatus Status { get; set; }
            = ApplicationStatus.Applied;

        public DateTime AppliedAt { get; set; }
            = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; }
            = DateTime.UtcNow;

        public Job Job { get; set; } = null!;

        public User Candidate { get; set; } = null!;
    }
}