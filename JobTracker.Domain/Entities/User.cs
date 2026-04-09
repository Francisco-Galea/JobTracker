namespace JobTracker.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string FullName { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }

        private User() { }

        public static User Create(string email, string passwordHash, string fullName)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío.", nameof(email));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("El password no puede estar vacío.", nameof(passwordHash));

            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(fullName));

            return new User
            {
                Id = Guid.NewGuid(),
                Email = email.Trim().ToLowerInvariant(),
                PasswordHash = passwordHash,
                FullName = fullName.Trim(),
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
