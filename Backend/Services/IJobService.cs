using JobMatching.API.DTOs;

namespace JobMatching.API.Services
{
    public interface IJobService
    {
        Task<List<JobResponseDto>> SearchOpenAsync(
            string? keyword,
            string? location
        );

        Task<JobResponseDto?> GetPublicByIdAsync(int id);

        Task<List<JobResponseDto>> GetMineAsync(
            int recruiterId
        );

        Task<JobResponseDto?> CreateAsync(
            int recruiterId,
            CreateJobDto dto
        );

        Task<JobResponseDto?> UpdateAsync(
            int recruiterId,
            int jobId,
            UpdateJobDto dto
        );

        Task<bool> CloseAsync(
            int recruiterId,
            int jobId
        );
    }
}