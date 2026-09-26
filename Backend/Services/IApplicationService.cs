using JobMatching.API.DTOs;

namespace JobMatching.API.Services
{
    public interface IApplicationService
    {
        Task<ApplicationResponseDto?> ApplyAsync(
            int candidateId,
            int jobId,
            ApplyJobDto dto
        );

        Task<List<ApplicationResponseDto>>
            GetMyApplicationsAsync(int candidateId);

        Task<ApplicationResponseDto?>
            GetMyApplicationByIdAsync(
                int candidateId,
                int applicationId
            );

        Task<bool> WithdrawAsync(
            int candidateId,
            int applicationId
        );

        Task<List<ApplicationResponseDto>>
            GetJobApplicationsAsync(
                int recruiterId,
                int jobId
            );

        Task<ApplicationResponseDto?> UpdateStatusAsync(
            int recruiterId,
            int applicationId,
            UpdateApplicationStatusDto dto
        );
    }
}