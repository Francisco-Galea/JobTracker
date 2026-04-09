namespace JobTracker.Application.UseCases.RegisterUser
{
    public record RegisterUserCommand(
        string Email,
        string Password,
        string FullName
    );
}
