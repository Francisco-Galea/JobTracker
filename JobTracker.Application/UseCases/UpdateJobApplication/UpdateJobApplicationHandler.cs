using JobTracker.Application.Common.Exceptions;
using JobTracker.Application.DTOs;
using JobTracker.Domain.Interfaces;

namespace JobTracker.Application.UseCases.UpdateJobApplication
{
    public class UpdateJobApplicationHandler
    {
        private readonly IJobApplicationRepository repository;

        public UpdateJobApplicationHandler(IJobApplicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<JobApplicationDto> HandleAsync(
            UpdateJobApplicationCommand command,
            CancellationToken cancellationToken = default)
        {
            var app = await repository.GetByIdAsync(command.Id, cancellationToken);

            if (app is null)
                throw new NotFoundException(nameof(app), command.Id);

            app.Update(command.Company, command.Position, command.JobUrl, command.Notes);
            app.ChangeStatus(command.Status);

            await repository.UpdateAsync(app, cancellationToken);

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
