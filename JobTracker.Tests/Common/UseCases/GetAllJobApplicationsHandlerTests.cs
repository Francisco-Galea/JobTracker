using FluentAssertions;
using JobTracker.Application.UseCases.CreateJobApplication;
using JobTracker.Application.UseCases.GetAllJobApplications;
using JobTracker.Tests.Common.Fakes;

namespace JobTracker.Tests.Common.UseCases
{
    public class GetAllJobApplicationsHandlerTests
    {
        private readonly FakeJobApplicationRepository repository;
        private readonly CreateJobApplicationHandler createHandler;
        private readonly GetAllJobApplicationsHandler getAllHandler;
        private readonly Guid userId = Guid.NewGuid();

        public GetAllJobApplicationsHandlerTests()
        {
            repository = new FakeJobApplicationRepository();
            createHandler = new CreateJobApplicationHandler(repository);
            getAllHandler = new GetAllJobApplicationsHandler(repository);
        }

        [Fact]
        public async Task Handle_WithNoApplications_ReturnsEmptyList()
        {
            // Arrange
            var query = new GetAllJobApplicationsQuery(userId);

            // Act
            var result = await getAllHandler.HandleAsync(query);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_WithApplications_ReturnsOnlyUserApplications()
        {
            // Arrange — creamos postulaciones para dos usuarios distintos
            var otherUserId = Guid.NewGuid();

            await createHandler.HandleAsync(
                new CreateJobApplicationCommand("Globant", "Dev", null, null, userId));
            await createHandler.HandleAsync(
                new CreateJobApplicationCommand("Accenture", "Dev", null, null, userId));
            await createHandler.HandleAsync(
                new CreateJobApplicationCommand("IBM", "Dev", null, null, otherUserId));

            // Act
            var result = await getAllHandler.HandleAsync(
                new GetAllJobApplicationsQuery(userId));

            // Assert — solo debe devolver las dos del usuario correcto
            result.Should().HaveCount(2);
            result.Should().AllSatisfy(x => x.Company.Should().NotBe("IBM"));
        }
    }
}
