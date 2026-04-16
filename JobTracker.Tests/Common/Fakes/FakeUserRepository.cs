using JobTracker.Domain.Entities;
using JobTracker.Domain.Interfaces;

namespace JobTracker.Tests.Common.Fakes
{
    public class FakeUserRepository : IUserRepository
    {
        private readonly List<User> store = new();

        public Task<User?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            var result = store.FirstOrDefault(
                x => x.Email == email.ToLowerInvariant());
            return Task.FromResult(result);
        }

        public Task<bool> ExistsByEmailAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            var exists = store.Any(x => x.Email == email.ToLowerInvariant());
            return Task.FromResult(exists);
        }

        public Task AddAsync(
            User user,
            CancellationToken cancellationToken = default)
        {
            store.Add(user);
            return Task.CompletedTask;
        }

        public int Count() => store.Count;
    }
}
