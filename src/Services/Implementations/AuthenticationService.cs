using System.Security.Claims;
using API.Infra.Exceptions;
using API.Infra.Authentication;
using API.Infra.Extensions;
using API.Infra.Database;
using API.Services.Interfaces;
using API.Repositories;

namespace API.Services.Implementations
{
    public partial class AuthenticationService : IAuthenticationService
    {
        private readonly UserRepository _userRepository;

        private readonly DataBaseTransaction _transaction;

        public AuthenticationService(IServiceProvider provider)
        {
            _transaction = provider.GetService<DataBaseTransaction>();
            _userRepository = provider.GetService<UserRepository>();
        }

        public string GetToken(string login, string password)
        {
            var user = _userRepository
                .Where(x => x.Login == login)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    Login = x.Login,
                    Profile = x.Profile,
                    Password = x.Password,
                })
                .FirstOrDefault();

            if (user == null)
                throw new BusinessException("Login ou senha incorretos");

            if(!Infra.Utils.BCrypt.IsValidPassword(password, user.Password))
                throw new BusinessException("Login ou senha incorretos");

            string token = TokenGenerator.CreateToken(new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Login),
                new Claim(ClaimTypes.Role, user.Profile.GetDescription()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            },
            DateTime.UtcNow.AddHours(1));

            return token;
        }
    }
}
