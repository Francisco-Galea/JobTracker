namespace JobTracker.Application.DTOs.Analytics
{
    public record ApplicationsByMonthDto(
        int Year,
        int Month,
        string MonthName,
        int Count
    );
}
