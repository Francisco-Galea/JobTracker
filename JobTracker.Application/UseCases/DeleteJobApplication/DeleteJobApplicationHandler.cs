using JobTracker.Application.Common.Exceptions;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Interfaces;

namespace JobTracker.Application.UseCases.DeleteJobApplication
{
    public class DeleteJobApplicationHandler
    {
        private readonly IJobApplicationRepository repository;

        public DeleteJobApplicationHandler(IJobApplicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task HandleAsync(
            DeleteJobApplicationCommand command,
            CancellationToken cancellationToken = default)
        {
            var app = await repository.GetByIdAsync(command.Id, cancellationToken);

            if (app is null || app.UserId != command.UserId)
                throw new NotFoundException(nameof(JobApplication), command.Id);

            await repository.DeleteAsync(app, cancellationToken);
        }
    }
}
