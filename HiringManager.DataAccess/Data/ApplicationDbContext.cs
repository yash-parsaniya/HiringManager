using Microsoft.EntityFrameworkCore;
using HiringManager.Models;

namespace HiringManager.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationDetails> ApplicationDetails { get; set; }
        public DbSet<PersonalDetails> PersonalDetails { get; set; }
        public DbSet<EducationDetails> EducationDetails { get; set; }
        public DbSet<ExperienceDetails> ExperienceDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<ApplicationDetails>()
                .HasOne(a => a.PersonalDetails)
                .WithOne(p => p.ApplicationDetails)
                .HasForeignKey<PersonalDetails>(p => p.ApplicationDetailsId);

            modelBuilder.Entity<ApplicationDetails>()
                .HasOne(a => a.EducationDetails)
                .WithOne(e => e.ApplicationDetails)
                .HasForeignKey<EducationDetails>(e => e.ApplicationDetailsId);

            modelBuilder.Entity<ApplicationDetails>()
                .HasOne(a => a.ExperienceDetails)
                .WithOne(e => e.ApplicationDetails)
                .HasForeignKey<ExperienceDetails>(e => e.ApplicationDetailsId);
        }
    }
}