using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeGame.DomainsModels;

namespace TicTacToeGame.Repositories
{
    public interface IGameRepository
    {
        public Task<IEnumerable<Game>> GetAll();
        public Task<Game> GetOne(int id);
        public Task<Game> SaveAsync(Game game);
        public Task DeleteAsync(Game game);
        public Task<Game> UpdateAsync(Game game);
    }
}
