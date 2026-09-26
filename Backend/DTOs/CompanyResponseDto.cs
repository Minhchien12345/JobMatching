namespace JobMatching.API.DTOs
{
    public class CompanyResponseDto
    {
        public int Id { get; set; }

        public int RecruiterId { get; set; }

        public string RecruiterName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Industry { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Website { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}