namespace JobMatching.API.DTOs
{
    public class RecruiterApplicantDetailDto
    {
        public ApplicationResponseDto Application { get; set; }
            = null!;

        public CandidateProfileResponseDto? Profile { get; set; }

        public List<CandidateSkillResponseDto> Skills { get; set; }
            = new();

        public MatchResultDto? MatchResult { get; set; }
    }
}