using JobTracker.Domain.Entities;

namespace JobTracker.Domain.Interfaces
{
    public interface IAnalyticsRepository
    {
        Task<IEnumerable<JobApplication>> GetAllByUserAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
