using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using TicTacToeGame.DomainsModels;

namespace TicTacToeGame.Repositories
{
    public class TicTacToeDbContext : IdentityDbContext<IdentityUser>
    {
        public TicTacToeDbContext(DbContextOptions options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .Property(g => g.GameState)
                .HasConversion<string>();

            modelBuilder.Entity<Cell>()
                .Property(c => c.CellState)
                .HasConversion<string>();

            modelBuilder.Entity<Game>()
                .HasOne(g => g.TicTacToe)
                .WithOne(t => t.Game)
                .HasForeignKey<TicTacToeGame.DomainsModels.TicTacToe>(t => t.GameId);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Game> Games { get; set; }
    }
}
