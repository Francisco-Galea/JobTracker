using JobTracker.Domain.Entities;
using JobTracker.Domain.Interfaces;

namespace JobTracker.Tests.Common.Fakes
{
    public class FakeJobApplicationRepository : IJobApplicationRepository
    {
        private readonly List<JobApplication> store = new();

        public Task<JobApplication?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var result = store.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(result);
        }

        public Task<IEnumerable<JobApplication>> GetAllAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var result = store.Where(x => x.UserId == userId);
            return Task.FromResult(result);
        }

        public Task AddAsync(
            JobApplication jobApplication,
            CancellationToken cancellationToken = default)
        {
            store.Add(jobApplication);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(
            JobApplication jobApplication,
            CancellationToken cancellationToken = default)
        {
            var existing = store.FirstOrDefault(x => x.Id == jobApplication.Id);
            if (existing != null)
            {
                store.Remove(existing);
                store.Add(jobApplication);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(
            JobApplication jobApplication,
            CancellationToken cancellationToken = default)
        {
            store.Remove(jobApplication);
            return Task.CompletedTask;
        }

        public int Count() => store.Count;
    }
}
