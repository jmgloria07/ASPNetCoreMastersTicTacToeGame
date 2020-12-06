using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using TicTacToeGame.Api.Properties;
using TicTacToeGame.Api.Infrastructures;
using TicTacToeGame.Repositories;
using TicTacToeGame.Services;
using TicTacToeGame.Services.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace TicTacToeGame.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<TicTacToeExceptionFilter>();
            });

            string connection = Configuration.GetConnectionString("SqlConnectionString");
            services.AddDbContext<TicTacToeDbContext>(options => options.UseSqlServer(connection, b => b.MigrationsAssembly("TicTacToeGame.Repositories")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<TicTacToeDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<TicTacToeDbContext>(options => {
                options.Database.Migrate();
            });

            services.AddSwaggerGen();

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Authentication:Jwt:SecurityKey"]));
            services.Configure<JwtOptions>(options => 
                options.SecurityKey = securityKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        IssuerSigningKey = securityKey
                    };
                }
            );

            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserShouldBeInvolved", policyBuilder =>
                {
                    policyBuilder.AddRequirements(new IsUserInvolved());
                });
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddSingleton<IAuthorizationHandler, IsUserInvolvedAuthorizationHandler>();
            services.AddSingleton<TicTacToeHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<TicTacToeDbContext>();
                context.Database.Migrate();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options => { 
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "TicTacToe Game API v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapControllers();
            });
        }
    }
}
