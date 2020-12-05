using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TicTacToeGame.Services.Dto;

namespace TicTacToeGame.Services
{
    public interface IGameService
    {
        public Task<GameDTO> Save(GameDTO dto);
        public Task<GameDTO> GetGame(int id);
        public Task<GameDTO> CastTurn(TurnDto turnDto);
        public IEnumerable<GameDTO> GetGames();
    }
}
