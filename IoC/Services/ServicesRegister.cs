using Application.Services.Implementations;
using Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IoC
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
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<ISessionService, SessionService>();
        }
    }
}
