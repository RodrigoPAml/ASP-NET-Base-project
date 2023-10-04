using Domain.Security;
using Infra.Security;
using Microsoft.Extensions.DependencyInjection;

namespace IoC
{
    public static class SecurityRegister
    {
        /// <summary>
        /// Register repositories
        /// </summary>
        /// <param name="services"></param>
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IPasswordHasherProvider, PasswordHasherProvider>();
        }
    }
}
