using Domain.Repositories.Interfaces;
using Infra.Repositories.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace IoC
{
    public static class RepositoriesRegister
    {
        /// <summary>
        /// Register repositories
        /// </summary>
        /// <param name="services"></param>
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
        }
    }
}
