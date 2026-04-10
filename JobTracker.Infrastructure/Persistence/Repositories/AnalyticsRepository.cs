using JobTracker.Domain.Entities;
using JobTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Persistence.Repositories
{
    internal class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly AppDbContext context;

        public AnalyticsRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<JobApplication>> GetAllByUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await context.JobApplications
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
