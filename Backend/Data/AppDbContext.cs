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
        }
    }
}