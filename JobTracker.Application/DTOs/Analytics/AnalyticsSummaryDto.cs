namespace JobTracker.Application.DTOs.Analytics
{
    public record AnalyticsSummaryDto(
        int TotalApplications,
        int ActiveApplications,
        int RejectedApplications,
        int OffersReceived,
        double ResponseRate,
        double? AverageDaysToResponse,
        IEnumerable<ApplicationsByStatusDto> ByStatus,
        IEnumerable<ApplicationsByMonthDto> ByMonth
    );
}
