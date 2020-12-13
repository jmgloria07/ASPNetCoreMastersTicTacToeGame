using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToeGame.Repositories;

namespace TicTacToeGame.Api.ServiceConfiguration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            string connection = config.GetConnectionString("SqlConnectionString");
            services.AddDbContext<TicTacToeDbContext>(options => 
                options.UseSqlServer(connection, 
                    b => b.MigrationsAssembly("TicTacToeGame.Repositories")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<TicTacToeDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<TicTacToeDbContext>(options => {
                options.Database.Migrate();
            });

            return services;
        }
    }
}
