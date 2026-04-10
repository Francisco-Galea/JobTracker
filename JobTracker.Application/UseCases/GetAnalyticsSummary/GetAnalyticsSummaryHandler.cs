using JobTracker.Application.DTOs.Analytics;
using JobTracker.Domain.Enums;
using JobTracker.Domain.Interfaces;

namespace JobTracker.Application.UseCases.GetAnalyticsSummary
{
    public class GetAnalyticsSummaryHandler
    {
        private readonly IAnalyticsRepository analyticsRepository;

        public GetAnalyticsSummaryHandler(IAnalyticsRepository analyticsRepository)
        {
            this.analyticsRepository = analyticsRepository;
        }

        public async Task<AnalyticsSummaryDto> HandleAsync(
            GetAnalyticsSummaryQuery query,
            CancellationToken cancellationToken = default)
        {
            var applications = (await analyticsRepository
                .GetAllByUserAsync(query.UserId, cancellationToken))
                .ToList();

            if (applications.Count == 0)
                return EmptySummary();

            var total = applications.Count;

            var active = applications.Count(x =>
                x.Status == ApplicationStatus.Applied ||
                x.Status == ApplicationStatus.InProcess);

            var rejected = applications.Count(x =>
                x.Status == ApplicationStatus.Rejected);

            var offers = applications.Count(x =>
                x.Status == ApplicationStatus.Offer ||
                x.Status == ApplicationStatus.Accepted);

            var withResponse = applications.Count(x =>
                x.Status != ApplicationStatus.Applied);

            var responseRate = total > 0
                ? Math.Round((double)withResponse / total * 100, 1)
                : 0;

            var respondedApps = applications
                .Where(x => x.LastUpdatedAt.HasValue &&
                            x.Status != ApplicationStatus.Applied)
                .ToList();

            double? avgDays = respondedApps.Count > 0
                ? Math.Round(respondedApps
                    .Average(x => (x.LastUpdatedAt!.Value - x.AppliedAt).TotalDays), 1)
                : null;

            var byStatus = applications
                .GroupBy(x => x.Status)
                .Select(g => new ApplicationsByStatusDto(
                    g.Key.ToString(),
                    g.Count(),
                    Math.Round((double)g.Count() / total * 100, 1)
                ))
                .OrderByDescending(x => x.Count)
                .ToList();

            var byMonth = applications
                .GroupBy(x => new { x.AppliedAt.Year, x.AppliedAt.Month })
                .Select(g => new ApplicationsByMonthDto(
                    g.Key.Year,
                    g.Key.Month,
                    new DateTime(g.Key.Year, g.Key.Month, 1)
                        .ToString("MMMM"),
                    g.Count()
                ))
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            return new AnalyticsSummaryDto(
                total,
                active,
                rejected,
                offers,
                responseRate,
                avgDays,
                byStatus,
                byMonth
            );
        }

        private static AnalyticsSummaryDto EmptySummary() =>
            new(0, 0, 0, 0, 0, null,
                Enumerable.Empty<ApplicationsByStatusDto>(),
                Enumerable.Empty<ApplicationsByMonthDto>());
    }
}
