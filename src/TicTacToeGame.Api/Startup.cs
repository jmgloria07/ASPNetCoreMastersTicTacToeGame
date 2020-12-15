using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicTacToeGame.Api.Middlewares;
using TicTacToeGame.Api.ServiceConfiguration;
using TicTacToeGame.Repositories;

namespace TicTacToeGame.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAppServices(_configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<TicTacToeDbContext>();
                context.Database.EnsureDeleted();
                context.Database.Migrate();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options => { 
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "TicTacToe API");
                options.RoutePrefix = string.Empty;
            });

            //custom middleware
            app.UseExceptionMiddleware();
            app.UseLogGetRequestMiddleware();

            app.UseRouting();

            app.UseCors(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
