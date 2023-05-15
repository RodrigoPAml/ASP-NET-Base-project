using API.Models.Entities;
using API.Repositories;

namespace API.Infra.Registration
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
        }
    }
}
