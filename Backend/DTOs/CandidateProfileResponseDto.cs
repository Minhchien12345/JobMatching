namespace JobMatching.API.DTOs
{
    public class CandidateProfileResponseDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Headline { get; set; } = string.Empty;

        public string Bio { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Education { get; set; } = string.Empty;

        public int YearsOfExperience { get; set; }
    }
}