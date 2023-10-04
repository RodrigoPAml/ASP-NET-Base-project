using Domain.Persistance;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infra.Database;
using Infra.Persistance;

namespace IoC
{
    public static class DatabaseRegister
    {
        /// <summary>
        /// Register repositories
        /// </summary>
        /// <param name="services"></param>
        public static void Register(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddScoped<IDatabaseTransaction, DatabaseTransaction>();
        }
    }
}
