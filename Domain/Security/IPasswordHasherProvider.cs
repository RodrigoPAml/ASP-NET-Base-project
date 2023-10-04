namespace Domain.Security
{
    public interface IPasswordHasherProvider
    {
        bool IsValid(string password, string hash);

        string Hash(string password);
    }
}
