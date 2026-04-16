using FluentAssertions;
using JobTracker.Application.Common.Exceptions;
using JobTracker.Application.UseCases.CreateJobApplication;
using JobTracker.Application.UseCases.DeleteJobApplication;
using JobTracker.Tests.Common.Fakes;

namespace JobTracker.Tests.Common.UseCases
{
    public class DeleteJobApplicationHandlerTests
    {
        private readonly FakeJobApplicationRepository repository;
        private readonly CreateJobApplicationHandler createHandler;
        private readonly DeleteJobApplicationHandler deleteHandler;
        private readonly Guid userId = Guid.NewGuid();

        public DeleteJobApplicationHandlerTests()
        {
            repository = new FakeJobApplicationRepository();
            createHandler = new CreateJobApplicationHandler(repository);
            deleteHandler = new DeleteJobApplicationHandler(repository);
        }

        [Fact]
        public async Task Handle_WithValidId_DeletesApplication()
        {
            // Arrange
            var created = await createHandler.HandleAsync(
                new CreateJobApplicationCommand(
                    "Mercado Libre", "Backend Dev", null, null, userId));

            // Act
            await deleteHandler.HandleAsync(
                new DeleteJobApplicationCommand(created.Id, userId));

            // Assert
            repository.Count().Should().Be(0);
        }

        [Fact]
        public async Task Handle_WithNonExistentId_ThrowsNotFoundException()
        {
            // Arrange
            var command = new DeleteJobApplicationCommand(Guid.NewGuid(), userId);

            // Act
            var action = () => deleteHandler.HandleAsync(command);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Handle_WithOtherUsersApplication_ThrowsNotFoundException()
        {
            // Arrange — creamos una postulación de otro usuario
            var otherUserId = Guid.NewGuid();
            var created = await createHandler.HandleAsync(
                new CreateJobApplicationCommand(
                    "Globant", "Dev", null, null, otherUserId));

            // Act — intentamos borrarla con nuestro userId
            var action = () => deleteHandler.HandleAsync(
                new DeleteJobApplicationCommand(created.Id, userId));

            // Assert — debe fallar aunque el ID exista
            await action.Should().ThrowAsync<NotFoundException>();
        }
    }
}
