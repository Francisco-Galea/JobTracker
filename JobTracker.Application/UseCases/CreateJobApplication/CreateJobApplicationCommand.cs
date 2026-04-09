namespace JobTracker.Application.UseCases.CreateJobApplication
{
    public record CreateJobApplicationCommand(
        string Company,
        string Position,
        string? JobUrl,
        string? Notes,
        Guid UserId = default              
    );
}
