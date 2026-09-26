using JobMatching.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JobMatching.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options
        ) : base(options)
        {
        }

        public DbSet<Skill> Skills { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<CandidateProfile> CandidateProfiles { get; set; } = null!;

        public DbSet<CandidateSkill> CandidateSkills { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Job> Jobs { get; set; } = null!;

        public DbSet<JobSkill> JobSkills { get; set; } = null!;
        public DbSet<JobApplication> JobApplications { get; set; } = null!;

        protected override void OnModelCreating(
            ModelBuilder modelBuilder
        )
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(user => user.Email)
                .IsUnique();

            modelBuilder.Entity<CandidateProfile>()
                .HasIndex(profile => profile.UserId)
                .IsUnique();

            modelBuilder.Entity<CandidateProfile>()
                .HasOne(profile => profile.User)
                .WithOne()
                .HasForeignKey<CandidateProfile>(
                    profile => profile.UserId
                )
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CandidateSkill>()
                .HasKey(candidateSkill => new
                {
                    candidateSkill.UserId,
                    candidateSkill.SkillId
                });

            modelBuilder.Entity<CandidateSkill>()
                .HasOne(candidateSkill => candidateSkill.User)
                .WithMany()
                .HasForeignKey(candidateSkill => candidateSkill.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CandidateSkill>()
                .HasOne(candidateSkill => candidateSkill.Skill)
                .WithMany()
                .HasForeignKey(candidateSkill => candidateSkill.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CandidateSkill>()
                .Property(candidateSkill =>
                    candidateSkill.YearsOfExperience
                )
                .HasPrecision(4, 1);

            modelBuilder.Entity<Company>()
                .HasIndex(company => company.RecruiterId)
                .IsUnique();

            modelBuilder.Entity<Company>()
                .HasOne(company => company.Recruiter)
                .WithOne()
                .HasForeignKey<Company>(
                    company => company.RecruiterId
                )
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Job>()
                .HasOne(job => job.Company)
                .WithMany()
                .HasForeignKey(job => job.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Job>()
                .Property(job => job.MinSalary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Job>()
                .Property(job => job.MaxSalary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<JobSkill>()
                .HasKey(jobSkill => new
                {
                    jobSkill.JobId,
                    jobSkill.SkillId
                });

            modelBuilder.Entity<JobSkill>()
                .HasOne(jobSkill => jobSkill.Job)
                .WithMany(job => job.JobSkills)
                .HasForeignKey(jobSkill => jobSkill.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobSkill>()
                .HasOne(jobSkill => jobSkill.Skill)
                .WithMany()
                .HasForeignKey(jobSkill => jobSkill.SkillId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobApplication>()
               .HasIndex(application => new{application.JobId,application.CandidateId}).IsUnique();

            modelBuilder.Entity<JobApplication>()
                .HasOne(application => application.Job)
                .WithMany()
                .HasForeignKey(application => application.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobApplication>()
                .HasOne(application => application.Candidate)
                .WithMany()
                .HasForeignKey(application => application.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}