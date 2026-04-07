using JobTracker.Application.DTOs;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Interfaces;

namespace JobTracker.Application.UseCases.CreateJobApplication
{
    public class CreateJobApplicationHandler
    {

        private readonly IJobApplicationRepository repository;

        public CreateJobApplicationHandler(IJobApplicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<JobApplicationDto> HandleAsync(
        CreateJobApplicationCommand command,
        CancellationToken cancellationToken = default)
        {
            var jobApplication = JobApplication.Create(
                command.Company,
                command.Position,
                command.JobUrl,
                command.Notes
            );

            await repository.AddAsync(jobApplication, cancellationToken);

            return MapToDto(jobApplication);
        }

        private static JobApplicationDto MapToDto(JobApplication app) =>
            new(
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
