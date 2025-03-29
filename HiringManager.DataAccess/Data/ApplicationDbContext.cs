using HiringManager.Models;
using Microsoft.EntityFrameworkCore;

namespace HiringManager.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }

        public DbSet<ApplicationDetail> ApplicationSet { get; set; }
        public DbSet<PersonalDetail> PersonalSet{ get; set; }
        public DbSet<EducationDetail> EducationSet { get; set; }
        public DbSet<ExperienceDetail> ExperienceSet { get; set; }
    }
}
