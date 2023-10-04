using Application.AppServices.Implementations;
using Application.AppServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IoC
{
    public static class AppServicesRegister
    {
        /// <summary>
        /// Register services
        /// </summary>
        /// <param name="services"></param>
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<IMovieAppService, MovieAppService>();
            services.AddScoped<ISessionAppService, SessionAppService>();
        }
    }
}
