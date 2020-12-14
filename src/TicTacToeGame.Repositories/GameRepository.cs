using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeGame.DomainsModels;

namespace TicTacToeGame.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly TicTacToeDbContext _dbContext;

        public GameRepository(TicTacToeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _dbContext.Games.Include(g => g.TicTacToe)
                    .ThenInclude(t => t.Cells).ToListAsync();
        }

        public async Task DeleteAsync(Game game)
        {
            _dbContext.Remove(game);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Game> GetByIdAsync(int id)
        {
            IQueryable<Game> query = _dbContext.Games.Include(g => g.TicTacToe)
                    .ThenInclude(t => t.Cells)
                    .Where(g => g.Id == id);
            
            if (query != null) return await query.FirstOrDefaultAsync();

            return null;
        }

        public async Task<Game> SaveAsync(Game game)
        {
            EntityEntry<Game> result = await _dbContext.Games.AddAsync(game);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Game> UpdateAsync(Game game)
        {
            EntityEntry<Game> result = _dbContext.Games.Update(game);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
    }
}
