using JobTracker.Domain.Entities;

namespace JobTracker.Domain.Interfaces
{
    public interface IJobApplicationRepository
    {
        Task<JobApplication?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<JobApplication>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(JobApplication jobApplication, CancellationToken cancellationToken = default);
        Task UpdateAsync(JobApplication jobApplication, CancellationToken cancellationToken = default);
        Task DeleteAsync(JobApplication jobApplication, CancellationToken cancellationToken = default);
    }
}
