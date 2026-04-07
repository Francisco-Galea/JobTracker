using JobTracker.Application.DTOs;
using JobTracker.Domain.Interfaces;

namespace JobTracker.Application.UseCases.GetAllJobApplications
{
    public class GetAllJobApplicationsHandler
    {
        private readonly IJobApplicationRepository repository;

        public GetAllJobApplicationsHandler(IJobApplicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<JobApplicationDto>> HandleAsync(
            GetAllJobApplicationsQuery query,
            CancellationToken cancellationToken = default)
        {
            var apps = await repository.GetAllAsync(cancellationToken);

            return apps.Select(app => new JobApplicationDto(
                app.Id,
                app.Company,
                app.Position,
                app.JobUrl,
                app.Notes,
                app.Status,
                app.Status.ToString(),
                app.AppliedAt,
                app.LastUpdatedAt
            ));
        }
    }
}
