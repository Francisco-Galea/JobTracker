using FluentAssertions;
using JobTracker.Application.Common.Exceptions;
using JobTracker.Application.UseCases.LoginUser;
using JobTracker.Application.UseCases.RegisterUser;
using JobTracker.Tests.Common.Fakes;

namespace JobTracker.Tests.Common.UseCases
{
    public class LoginUserHandlerTests
    {
        private readonly FakeUserRepository userRepository;
        private readonly FakePasswordHasher passwordHasher;
        private readonly FakeJwtTokenGenerator jwtGenerator;
        private readonly RegisterUserHandler registerHandler;
        private readonly LoginUserHandler loginHandler;

        public LoginUserHandlerTests()
        {
            userRepository = new FakeUserRepository();
            passwordHasher = new FakePasswordHasher();
            jwtGenerator = new FakeJwtTokenGenerator();
            registerHandler = new RegisterUserHandler(
                userRepository, passwordHasher, jwtGenerator);
            loginHandler = new LoginUserHandler(
                userRepository, passwordHasher, jwtGenerator);
        }

        [Fact]
        public async Task Handle_WithValidCredentials_ReturnsToken()
        {
            // Arrange — registramos un usuario primero
            await registerHandler.HandleAsync(new RegisterUserCommand(
                "francisco@gmail.com", "Password123!", "Francisco Galea"));

            // Act
            var result = await loginHandler.HandleAsync(new LoginUserCommand(
                "francisco@gmail.com", "Password123!"));

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Handle_WithWrongPassword_ThrowsUnauthorizedException()
        {
            // Arrange
            await registerHandler.HandleAsync(new RegisterUserCommand(
                "francisco@gmail.com", "Password123!", "Francisco Galea"));

            // Act
            var action = () => loginHandler.HandleAsync(new LoginUserCommand(
                "francisco@gmail.com", "WrongPassword"));

            // Assert
            await action.Should().ThrowAsync<UnauthorizedException>();
        }

        [Fact]
        public async Task Handle_WithNonExistentEmail_ThrowsUnauthorizedException()
        {
            // Act
            var action = () => loginHandler.HandleAsync(new LoginUserCommand(
                "noexiste@gmail.com", "Password123!"));

            // Assert
            await action.Should().ThrowAsync<UnauthorizedException>();
        }

        [Fact]
        public async Task Handle_WithEmailInDifferentCase_Succeeds()
        {
            // Arrange — registramos con minúsculas
            await registerHandler.HandleAsync(new RegisterUserCommand(
                "francisco@gmail.com", "Password123!", "Francisco Galea"));

            // Act — hacemos login con mayúsculas
            var action = () => loginHandler.HandleAsync(new LoginUserCommand(
                "FRANCISCO@GMAIL.COM", "Password123!"));

            // Assert — debe funcionar porque normalizamos el email a minúsculas
            await action.Should().NotThrowAsync();
        }
    }
}
