using JobMatching.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JobMatching.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Skill> Skills { get; set; } = null!;
    }
}