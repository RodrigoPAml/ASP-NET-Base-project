using Domain.Models.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace IoC
{
    public static class ValidatorsRegister
    {
        /// <summary>
        /// Register services
        /// </summary>
        /// <param name="services"></param>
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<MovieValidator>();
            services.AddScoped<UserValidator>();
        }
    }
}
