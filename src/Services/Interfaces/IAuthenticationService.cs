namespace API.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public string GetToken(string login, string password);
    }
}
