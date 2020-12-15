using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TicTacToeGame.Api.Properties;
using TicTacToeGame.Api.Utilities;
using TicTacToeGame.Configurations;

namespace TicTacToeGame.Api.ServiceConfiguration
{
    public static class AppServiceConfiguration
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<SmtpConfiguration>(config.GetSection("SmtpSettings"));
            services.AddControllerConfigurations();
            services.AddDatabase(config);
            services.AddSwagger();

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["Authentication:Jwt:SecurityKey"]));
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

            return services;
        }
    }
}
