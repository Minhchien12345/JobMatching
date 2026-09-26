using JobMatching.API.DTOs;

namespace JobMatching.API.Services
{
    public interface IMatchingService
    {
        Task<MatchResultDto?> CalculateAsync(
            int candidateId,
            int jobId
        );

        Task<List<MatchResultDto>>
            GetRecommendationsAsync(int candidateId);
    }
}