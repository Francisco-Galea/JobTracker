using JobTracker.Domain.Entities;
using JobTracker.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) { }

        public DbSet<JobApplication> JobApplications => Set<JobApplication>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new JobApplicationConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }

    }
}
