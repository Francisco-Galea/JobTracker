namespace JobTracker.Application.DTOs.Analytics
{
    public record ApplicationsByStatusDto(
        string Status,
        int Count,
        double Percentage
    );
}
