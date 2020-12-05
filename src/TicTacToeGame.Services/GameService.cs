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

        public IEnumerable<GameDTO> GetGames()
        {
            IList<GameDTO> games = new List<GameDTO>();
            
            foreach (Game game in _gameRepository.GetAll())
            {
                games.Add(new GameDTO() { 
                    Id = game.Id,
                    GameState = game.GameState,
                    OwnerId = game.OwnerId,
                    PlayerCross = game.PlayerCross,
                    PlayerNaught = game.PlayerNaught,
                    TicTacToe = new TicTacToeDto()
                    {
                        Cells = _ticTacToeHelper.PopulateCellDtos(game.TicTacToe.Cells)
                    }
                });
            }

            return games;
        }

        public async Task<GameDTO> GetGame(int id)
        {
            Game game = await _gameRepository.GetOne(id);
            if (game == null) return null;
            return new GameDTO
            {
                Id = game.Id,
                PlayerCross = game.PlayerCross,
                PlayerNaught = game.PlayerNaught,
                TicTacToe = new TicTacToeDto()
                {
                    Cells = _ticTacToeHelper.PopulateCellDtos(game.TicTacToe.Cells)
                },
                GameState = game.GameState
            };
        }

        public async Task<GameDTO> Save(GameDTO dto)
        {
            dto.GameState = Game.State.CROSS_TURN; //cross always starts
            Game game = await _gameRepository.SaveAsync(new Game()
            {
                PlayerCross = dto.PlayerCross,
                PlayerNaught = dto.PlayerNaught,
                OwnerId = dto.OwnerId,
                DateCreated = DateTime.UtcNow,
                TicTacToe = new TicTacToe()
                {
                    Cells = _ticTacToeHelper.InitializeCells()
                },
                GameState = dto.GameState
            });
            dto.Id = game.Id;
            dto.TicTacToe = new TicTacToeDto()
            {
                Cells = _ticTacToeHelper.PopulateCellDtos(game.TicTacToe.Cells)
            };
            return dto;
        }
        public async Task<GameDTO> CastTurn(TurnDto turnDto)
        {
            Game game = await _gameRepository.GetOne(turnDto.Game);

            Cell cell = _ticTacToeHelper.GetCellFromRowAndCol(
                game.TicTacToe.Cells, turnDto.Row, turnDto.Column);

            if (!_ticTacToeHelper.ShouldSetCell(game, turnDto.CastedBy, cell)) 
                throw new InputValidationException();

            cell.OwnerId = turnDto.CastedBy;
            game.TicTacToe = _ticTacToeHelper.CastTurn(game.TicTacToe, cell, game.GameState);

            game.GameState = _ticTacToeHelper.CalculateGameState(game.TicTacToe, cell.CellState);

            game = await _gameRepository.UpdateAsync(game);
            
            return new GameDTO()
            {
                Id = game.Id,
                GameState = game.GameState,
                PlayerCross = game.PlayerCross,
                PlayerNaught = game.PlayerNaught,
                TicTacToe = new TicTacToeDto()
                {
                    Cells = _ticTacToeHelper.PopulateCellDtos(game.TicTacToe.Cells)
                }
            };
        }
    }
}
