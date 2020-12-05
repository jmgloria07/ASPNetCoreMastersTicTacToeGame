using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using TicTacToe.DomainsModels;

namespace TicTacToe.Repositories
{
    public class TicTacToeDbContext : IdentityDbContext<IdentityUser>
    {
        public TicTacToeDbContext(DbContextOptions options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasOne(g => g.TicTacToe)
                .WithOne(t => t.Game)
                .HasForeignKey<TicTacToe.DomainsModels.TicTacToe>(t => t.GameId);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Game> Games { get; set; }
    }
}
