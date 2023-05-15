using API.Infra.Database;
using API.Infra.Authentication;
using API.Infra.Registration;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public static class Program
    {
        public static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("API Loading");

            var builder = WebApplication.CreateBuilder();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            TokenGenerator.BuildAuthentication(builder);
#if DEBUG
            SwaggerRegister.Register(builder.Services);
#endif
            RepositoriesRegister.Register(builder.Services);
            ServicesRegister.Register(builder.Services);

            builder.Services.AddDbContext<DataBaseContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddScoped<DataBaseTransaction>();

            var app = builder.Build();

#if DEBUG
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("In development mode");

            app.UseSwagger();
            app.UseSwaggerUI();
#else
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("In release mode");

            app.UseHttpsRedirection();
#endif
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("API Started");
            Console.ForegroundColor = ConsoleColor.White;

            app.Run();
        }
    }
}