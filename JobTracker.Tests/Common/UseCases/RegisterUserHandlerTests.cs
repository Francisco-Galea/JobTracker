using FluentAssertions;
using JobTracker.Application.Common.Exceptions;
using JobTracker.Application.UseCases.RegisterUser;
using JobTracker.Tests.Common.Fakes;

namespace JobTracker.Tests.Common.UseCases
{
    public class RegisterUserHandlerTests
    {
        private readonly FakeUserRepository userRepository;
        private readonly FakePasswordHasher passwordHasher;
        private readonly FakeJwtTokenGenerator jwtGenerator;
        private readonly RegisterUserHandler handler;

        public RegisterUserHandlerTests()
        {
            userRepository = new FakeUserRepository();
            passwordHasher = new FakePasswordHasher();
            jwtGenerator = new FakeJwtTokenGenerator();
            handler = new RegisterUserHandler(
                userRepository, passwordHasher, jwtGenerator);
        }

        [Fact]
        public async Task Handle_WithValidData_ReturnsAuthResponse()
        {
            // Arrange
            var command = new RegisterUserCommand(
                "francisco@gmail.com", "Password123!", "Francisco Galea");

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be("francisco@gmail.com");
            result.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Handle_WithValidData_PersistsUser()
        {
            // Arrange
            var command = new RegisterUserCommand(
                "francisco@gmail.com", "Password123!", "Francisco Galea");

            // Act
            await handler.HandleAsync(command);

            // Assert
            userRepository.Count().Should().Be(1);
        }

        [Fact]
        public async Task Handle_WithDuplicateEmail_ThrowsConflictException()
        {
            // Arrange — registramos el mismo email dos veces
            var command = new RegisterUserCommand(
                "francisco@gmail.com", "Password123!", "Francisco Galea");
            await handler.HandleAsync(command);

            // Act
            var action = () => handler.HandleAsync(command);

            // Assert
            await action.Should().ThrowAsync<ConflictException>();
        }
    }
}
