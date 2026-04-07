using JobTracker.Domain.Entities;
using JobTracker.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Persistence
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) { }

        public DbSet<JobApplication> JobApplications => Set<JobApplication>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new JobApplicationConfiguration());
            base.OnModelCreating(modelBuilder);
        }

    }
}
