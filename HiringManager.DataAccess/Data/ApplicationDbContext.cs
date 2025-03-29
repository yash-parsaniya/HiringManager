using HiringManager.Models;
using Microsoft.EntityFrameworkCore;

namespace HiringManager.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationDetail> ApplicationDetails { get; set; }
        public DbSet<PersonalDetail> PersonalDetails { get; set; }
        public DbSet<EducationDetail> EducationDetails { get; set; }
        public DbSet<ExperienceDetail> ExperienceDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<ApplicationDetail>()
                .HasOne(a => a.PersonalDetail)
                .WithOne()
                .HasForeignKey<PersonalDetail>(p => p.ApplicationDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationDetail>()
                .HasMany(a => a.EducationDetails)
                .WithOne(e => e.ApplicationDetail)
                .HasForeignKey(e => e.ApplicationDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationDetail>()
                .HasMany(a => a.ExperienceDetails)
                .WithOne(e => e.ApplicationDetail)
                .HasForeignKey(e => e.ApplicationDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint for application ID
            modelBuilder.Entity<ApplicationDetail>()
                .HasIndex(a => a.ApplicationId)
                .IsUnique();
        }
    }
}
