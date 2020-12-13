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
using Microsoft.OpenApi.Models;
using TicTacToeGame.Api.Middlewares;
using TicTacToeGame.Api.Properties;
using TicTacToeGame.Api.Utilities;
using TicTacToeGame.Repositories;

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
            }).AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                }               
            );

            string connection = Configuration.GetConnectionString("SqlConnectionString");
            services.AddDbContext<TicTacToeDbContext>(options => options.UseSqlServer(connection, b => b.MigrationsAssembly("TicTacToeGame.Repositories")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<TicTacToeDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<TicTacToeDbContext>(options => {
                options.Database.Migrate();
            });

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

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<TicTacToeDbContext>();
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
                    .AllowAnyMethod()
                    .AllowCredentials();
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
