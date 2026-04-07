using JobTracker.Domain.Enums;

namespace JobTracker.Domain.Entities
{
    public class JobApplication
    {
        public Guid Id { get; private set; }
        public string Company { get; private set; } = string.Empty;
        public string Position { get; private set; } = string.Empty;
        public string? JobUrl { get; private set; }
        public string? Notes { get; private set; }
        public ApplicationStatus Status { get; private set; }
        public DateTime AppliedAt { get; private set; }
        public DateTime? LastUpdatedAt { get; private set; }

        private JobApplication() { }

        public static JobApplication Create(
            string company,
            string position,
            string? jobUrl = null,
            string? notes = null)
        {
            if (string.IsNullOrWhiteSpace(company))
                throw new ArgumentException("La empresa no puede estar vacía.", nameof(company));

            if (string.IsNullOrWhiteSpace(position))
                throw new ArgumentException("El puesto no puede estar vacío.", nameof(position));

            return new JobApplication
            {
                Id = Guid.NewGuid(),
                Company = company.Trim(),
                Position = position.Trim(),
                JobUrl = jobUrl?.Trim(),
                Notes = notes?.Trim(),
                Status = ApplicationStatus.Applied,
                AppliedAt = DateTime.UtcNow
            };
        }

        public void Update(string company, string position, string? jobUrl, string? notes)
        {
            if (string.IsNullOrWhiteSpace(company))
                throw new ArgumentException("La empresa no puede estar vacía.", nameof(company));

            if (string.IsNullOrWhiteSpace(position))
                throw new ArgumentException("El puesto no puede estar vacío.", nameof(position));

            Company = company.Trim();
            Position = position.Trim();
            JobUrl = jobUrl?.Trim();
            Notes = notes?.Trim();
            LastUpdatedAt = DateTime.UtcNow;
        }

        public void ChangeStatus(ApplicationStatus newStatus)
        {
            Status = newStatus;
            LastUpdatedAt = DateTime.UtcNow;
        }
    }
}
