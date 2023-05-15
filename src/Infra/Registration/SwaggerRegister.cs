﻿using Microsoft.OpenApi.Models;

namespace API.Infra.Registration
{
    public static class SwaggerRegister
    {
        /// <summary>
        /// Register swagger
        /// </summary>
        /// <param name="services"></param>
        public static void Register(IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API",
                    Version = "v1"
                });

                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Description = "Insert the bearer token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }});
            });
        }
    }
}
