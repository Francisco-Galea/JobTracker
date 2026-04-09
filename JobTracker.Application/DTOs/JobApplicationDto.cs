using JobTracker.Domain.Enums;

namespace JobTracker.Application.DTOs
{
    public record JobApplicationDto(
        Guid Id,
        string Company,
        string Position,
        string? JobUrl,
        string? Notes,
        ApplicationStatus Status,
        string StatusDisplay,
        DateTime AppliedAt,
        DateTime? LastUpdatedAt
    );
}
