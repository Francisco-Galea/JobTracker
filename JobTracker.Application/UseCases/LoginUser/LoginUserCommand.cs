namespace JobTracker.Application.UseCases.LoginUser
{
    public record LoginUserCommand(
        string Email,
        string Password
    );
}
