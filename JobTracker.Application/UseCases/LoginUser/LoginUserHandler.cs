using JobTracker.Application.Common.Exceptions;
using JobTracker.Application.DTOs;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Interfaces;

namespace JobTracker.Application.UseCases.LoginUser
{
    public class LoginUserHandler
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly IJwtTokenGenerator jwtTokenGenerator;

        public LoginUserHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthDto> HandleAsync(
            LoginUserCommand command,
            CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByEmailAsync(
                command.Email, cancellationToken);

            if (user is null)
                throw new UnauthorizedException("Credenciales inválidas.");

            var isValid = passwordHasher.Verify(command.Password, user.PasswordHash);

            if (!isValid)
                throw new UnauthorizedException("Credenciales inválidas.");

            var token = jwtTokenGenerator.GenerateToken(user);

            return new AuthDto(user.Id, user.Email, user.FullName, token);
        }
    }
}
