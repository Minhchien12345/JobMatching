namespace JobMatching.API.DTOs
{
    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }

        public int TotalCandidates { get; set; }

        public int TotalRecruiters { get; set; }

        public int TotalCompanies { get; set; }

        public int ActiveCompanies { get; set; }

        public int TotalJobs { get; set; }

        public int OpenJobs { get; set; }

        public int TotalApplications { get; set; }
    }
}