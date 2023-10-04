using static BCrypt.Net.BCrypt;

namespace Infra.BCrypt
{
    /// <summary>
    /// Safe encryption for password and password break (brute force)
    /// </summary>
    public static class BCryptHasher
    {
        private static int WorkFactor = 12;

        public static string EncryptPassword(string password)
        {
            return HashPassword(password, WorkFactor);
        }

        public static bool IsValidPassword(string password, string hash)
        {
            return Verify(password, hash);
        }
    }
}
