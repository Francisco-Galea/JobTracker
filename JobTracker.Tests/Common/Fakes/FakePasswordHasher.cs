using JobTracker.Application.Interfaces;

namespace JobTracker.Tests.Common.Fakes
{
    public class FakePasswordHasher : IPasswordHasher
    {
        // En tests no necesitamos BCrypt real.
        // Simplemente devolvemos el password con un prefijo
        // para poder verificarlo después.
        public string Hash(string password) => $"hashed_{password}";

        public bool Verify(string password, string passwordHash) =>
            passwordHash == $"hashed_{password}";
    }
}
