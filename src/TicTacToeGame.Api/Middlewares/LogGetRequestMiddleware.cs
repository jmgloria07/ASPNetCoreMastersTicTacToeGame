using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame.Api.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LogGetRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogGetRequestMiddleware> _logger;

        public LogGetRequestMiddleware(RequestDelegate next, 
            ILogger<LogGetRequestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (HttpMethods.IsGet(context.Request.Method))
            {
                _logger.LogInformation($"Get request to {context.Request.Path}");
                _logger.LogInformation(GetRouteValues(context.Request.RouteValues));
            }

            await _next(context);
        }

        private string GetRouteValues(RouteValueDictionary routeValues)
        {
            StringBuilder sb = new StringBuilder("Route values: ");
            foreach (var value in routeValues)
            {
                sb.AppendLine($"key: {value.Key}, value: {value.Value}");
            }

            return sb.ToString();
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LogGetRequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogGetRequestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogGetRequestMiddleware>();
        }
    }
}
