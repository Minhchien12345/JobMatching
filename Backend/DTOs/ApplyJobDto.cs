using System.ComponentModel.DataAnnotations;

namespace JobMatching.API.DTOs
{
    public class ApplyJobDto
    {
        [MaxLength(2000)]
        public string CoverLetter { get; set; } = string.Empty;
    }
}