using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;

namespace JobTracker.Tests.Common.Fakes
{
    public class FakeJwtTokenGenerator : IJwtTokenGenerator
    {
        public string GenerateToken(User user) =>
            $"fake-token-{user.Id}";
    }
}
