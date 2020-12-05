using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TicTacToeGame.Repositories
{
    public class TicTacToeDbContextFactory : IDesignTimeDbContextFactory<TicTacToeDbContext>
    {
        public TicTacToeDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

            var dbContextBuilder = new DbContextOptionsBuilder();

            var connectionString = configuration
                        .GetConnectionString("SqlConnectionString");

            dbContextBuilder.UseSqlServer(connectionString);

            return new TicTacToeDbContext(dbContextBuilder.Options);
        }
    }
}
