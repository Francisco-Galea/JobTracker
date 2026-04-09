using JobTracker.Domain.Enums;

namespace JobTracker.Application.UseCases.UpdateJobApplication
{
    public record UpdateJobApplicationCommand(
        Guid Id,
        string Company,
        string Position,
        string? JobUrl,
        string? Notes,
        ApplicationStatus Status,
        Guid UserId = default
    );
}
