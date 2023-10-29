using Microsoft.AspNetCore.Builder;

namespace IoC
{
    public static class Register
    {
        public static void Process(WebApplicationBuilder builder)
        {
            DatabaseRegister.Register(builder);
#if DEBUG
            SwaggerRegister.Register(builder.Services);
#endif
            SecurityRegister.Register(builder.Services);  
            RepositoriesRegister.Register(builder.Services);
            ValidatorsRegister.Register(builder.Services);
            ServicesRegister.Register(builder.Services);
            AppServicesRegister.Register(builder.Services);
            JwtRegister.BuildAuthentication(builder);
        }
    }
}
