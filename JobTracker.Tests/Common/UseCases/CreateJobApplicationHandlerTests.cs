using FluentAssertions;
using JobTracker.Application.UseCases.CreateJobApplication;
using JobTracker.Domain.Enums;
using JobTracker.Tests.Common.Fakes;

namespace JobTracker.Tests.Common.UseCases
{
    public class CreateJobApplicationHandlerTests
    {
        private readonly FakeJobApplicationRepository repository;
        private readonly CreateJobApplicationHandler handler;
        private readonly Guid userId = Guid.NewGuid();

        public CreateJobApplicationHandlerTests()
        {
            this.repository = new FakeJobApplicationRepository();
            this.handler = new CreateJobApplicationHandler(repository);
        }

        [Fact]
        public async Task Handle_WithValidData_ReturnsDto()
        {
            // Arrange
            var command = new CreateJobApplicationCommand(
                "Mercado Libre", "Backend Developer", null, null, userId);

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            result.Should().NotBeNull();
            result.Company.Should().Be("Mercado Libre");
            result.Position.Should().Be("Backend Developer");
            result.Status.Should().Be(ApplicationStatus.Applied);
            result.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task Handle_WithValidData_PersistsInRepository()
        {
            // Arrange
            var command = new CreateJobApplicationCommand(
                "Globant", "NET Developer", null, null, userId);

            // Act
            await handler.HandleAsync(command);

            // Assert
            repository.Count().Should().Be(1);
        }

        [Fact]
        public async Task Handle_WithEmptyCompany_ThrowsArgumentException()
        {
            // Arrange
            var command = new CreateJobApplicationCommand(
                "", "Backend Developer", null, null, userId);

            // Act
            var action = () => handler.HandleAsync(command);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Handle_WithEmptyPosition_ThrowsArgumentException()
        {
            // Arrange
            var command = new CreateJobApplicationCommand(
                "Mercado Libre", "", null, null, userId);

            // Act
            var action = () => handler.HandleAsync(command);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }
    }
}
