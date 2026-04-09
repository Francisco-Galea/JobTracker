using JobTracker.Domain.Entities;
using JobTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Persistence.Repositories
{
    internal class JobApplicationRepository : IJobApplicationRepository
    {

        private readonly AppDbContext context;

        public JobApplicationRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<JobApplication?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        {
            return await context.JobApplications
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<JobApplication>> GetAllAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
        {
            return await context.JobApplications
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.AppliedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(
            JobApplication jobApplication,
            CancellationToken cancellationToken = default)
        {
            await context.JobApplications.AddAsync(jobApplication, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(
            JobApplication jobApplication,
            CancellationToken cancellationToken = default)
        {
            context.JobApplications.Update(jobApplication);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(
            JobApplication jobApplication,
            CancellationToken cancellationToken = default)
        {
            context.JobApplications.Remove(jobApplication);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
