using JobTracker.Application.Common.Exceptions;
using JobTracker.Application.DTOs;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Interfaces;

namespace JobTracker.Application.UseCases.GetJobApplication
{
    public class GetJobApplicationHandler
    {
        private readonly IJobApplicationRepository repository;

        public GetJobApplicationHandler(IJobApplicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<JobApplicationDto> HandleAsync(
            GetJobApplicationQuery query,
            CancellationToken cancellationToken = default)
        {
            var app = await repository.GetByIdAsync(query.Id, cancellationToken);

            if (app is null || app.UserId != query.UserId)
                throw new NotFoundException(nameof(JobApplication), query.Id);

            return new JobApplicationDto(
                app.Id,
                app.Company,
                app.Position,
                app.JobUrl,
                app.Notes,
                app.Status,
                app.Status.ToString(),
                app.AppliedAt,
                app.LastUpdatedAt
            );
        }
    }
}
