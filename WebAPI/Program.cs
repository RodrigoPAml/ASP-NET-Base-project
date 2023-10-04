using IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI
{
    public static class Program
    {
        public static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("API Loading");

            WebApplicationBuilder builder = WebApplication.CreateBuilder();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            Register.Process(builder);
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