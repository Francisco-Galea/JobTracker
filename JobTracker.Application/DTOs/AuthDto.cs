namespace JobTracker.Application.DTOs
{
    public record AuthDto(
    Guid UserId,
    string Email,
    string FullName,
    string Token
    );
}
