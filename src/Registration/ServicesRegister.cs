using API.Services.Implementations;
using API.Services.Interfaces;

namespace API.Registration
{
    public static class ServicesRegister
    {
        /// <summary>
        /// Register services
        /// </summary>
        /// <param name="services"></param>
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }
    }
}
