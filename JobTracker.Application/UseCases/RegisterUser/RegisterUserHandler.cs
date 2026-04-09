using JobTracker.Application.Common.Exceptions;
using JobTracker.Application.DTOs;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Interfaces;

namespace JobTracker.Application.UseCases.RegisterUser
{
    public class RegisterUserHandler
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly IJwtTokenGenerator jwtTokenGenerator;

        public RegisterUserHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthDto> HandleAsync(
            RegisterUserCommand command,
            CancellationToken cancellationToken = default)
        {
            var exists = await userRepository.ExistsByEmailAsync(
                command.Email, cancellationToken);

            if (exists)
                throw new ConflictException("Ya existe un usuario con ese email.");

            var passwordHash = passwordHasher.Hash(command.Password);

            var user = User.Create(command.Email, passwordHash, command.FullName);

            await userRepository.AddAsync(user, cancellationToken);

            var token = jwtTokenGenerator.GenerateToken(user);

            return new AuthDto(user.Id, user.Email, user.FullName, token);
        }
    }
}
