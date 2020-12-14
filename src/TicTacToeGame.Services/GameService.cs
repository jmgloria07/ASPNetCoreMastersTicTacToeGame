using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToeGame.DomainsModels;
using TicTacToeGame.Repositories;
using TicTacToeGame.Services.Dto;
using TicTacToeGame.Services.Exceptions;
using TicTacToeGame.Services.Utilities;

namespace TicTacToeGame.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly TicTacToeHelper _ticTacToeHelper;
        public GameService(IGameRepository gameRepository,
            TicTacToeHelper ticTacToeHelper)
        {
            _gameRepository = gameRepository;
            _ticTacToeHelper = ticTacToeHelper;
        }

        public async Task<IEnumerable<Game>> GetGames()
        {
            return await _gameRepository.GetAllAsync();
        }

        public async Task<Game> GetGame(int id)
        {
            return await _gameRepository.GetByIdAsync(id);
        }

        public async Task<Game> CreateGame(Game game)
        {
            game.DateCreated = DateTime.UtcNow;
            game.TicTacToe = new TicTacToe()
            {
                Cells = _ticTacToeHelper.InitializeCells(game.OwnerId)
            };
            game.GameState = Game.State.CROSS_TURN; //cross always starts

            game = await _gameRepository.SaveAsync(game);
            
            return game;
        }
        public async Task<Game> CastTurn(TurnDto turnDto)
        {
            Game game = await _gameRepository.GetByIdAsync(turnDto.Game);

            Cell cell = _ticTacToeHelper.GetCellFromRowAndCol(
                game.TicTacToe.Cells, turnDto.Row, turnDto.Column);

            if (!_ticTacToeHelper.ShouldSetCell(game, turnDto.CastedBy, cell)) 
                throw new TurnUncastedException();

            cell.CastedBy = turnDto.CastedBy;

            game.TicTacToe = _ticTacToeHelper.CastTurn(game.TicTacToe, cell, game.GameState);

            game.GameState = _ticTacToeHelper.CalculateGameState(game.TicTacToe, cell.CellState);

            return await _gameRepository.UpdateAsync(game);
        }

        public async Task DeleteGame(Game game)
        {
            await _gameRepository.DeleteAsync(game);
        }
    }
}
