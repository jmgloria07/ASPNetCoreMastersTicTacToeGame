using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToeGame.Api.ServiceConfiguration
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASPNetCoreMasters Final Project: TicTacToe API",
                    Description = "A simple TicTacToe game utilizing ASP.NET Core Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "Josh Gloria, Russel Punio, Rex Reyes III",
                        Email = string.Empty,
                        Url = new Uri("https://tictactoe.quantumjosh.me"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Licensed under MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT"),
                    }
                });
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });
            });

            return services;
        }
    }
}
