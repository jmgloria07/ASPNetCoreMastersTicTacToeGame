using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TicTacToeGame.DomainsModels;
using TicTacToeGame.Services.Dto;

namespace TicTacToeGame.Services
{
    public interface IGameService
    {
        public Task<Game> CreateGame(Game dto);
        public Task<Game> GetGame(int id);
        public Task<Game> CastTurn(TurnDto turnDto);
        public Task<IEnumerable<Game>> GetGames();
    }
}
