using static BCrypt.Net.BCrypt;

namespace API.Infra.Utils
{
    /// <summary>
    /// Safe encryption for password and password break (brute force)
    /// </summary>
    public static class BCrypt
    {
        private static int WorkFactor = 11;

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
