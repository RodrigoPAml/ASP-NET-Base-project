using API.Repositories;

namespace API.Registration
{
    public static class RepositoriesRegister
    {
        /// <summary>
        /// Register repositories
        /// </summary>
        /// <param name="services"></param>
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<UserRepository>();
            services.AddScoped<MovieRepository>();
            services.AddScoped<SessionRepository>();
        }
    }
}
