using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToeGame.Api.Utilities;

namespace TicTacToeGame.Api.ServiceConfiguration
{
    public static class ControllerConfiguration
    {
        public static IServiceCollection AddControllerConfigurations(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<TicTacToeExceptionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            return services;
        }
    }
}
