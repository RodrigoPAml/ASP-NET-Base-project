using Domain.Security;
using Infra.BCrypt;

namespace Infra.Security
{
    public class PasswordHasherProvider : IPasswordHasherProvider
    {
        public string Hash(string password)
        {
            return BCryptHasher.EncryptPassword(password);
        }

        public bool IsValid(string password, string hash)
        {
            return BCryptHasher.IsValidPassword(password, hash);
        }
    }
}
