using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicTacToeGame.Api.Models;
using TicTacToeGame.Services;
using TicTacToeGame.Services.Dto;
using TicTacToeGame.Services.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicTacToeGame.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }
        // GET: api/<GamesController>
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<GameDTO> games = _gameService.GetGames();
            if (games == null) return NotFound();
            return CustomGamesOk($"Found {games.Count()} games", games);
        }

        // GET api/<GamesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            GameDTO game = await _gameService.GetGame(id);
            if (game == null) return NotFound();
            return CustomGamesOk($"Found game with ID: {id}.", 
                new GameDTO[] { game });
        }

        // POST api/<GamesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateGameApiModel model)
        {
            GameDTO game = await _gameService.Save(
                new GameDTO()
                {
                    PlayerCross = model.PlayerCross,
                    PlayerNaught = model.PlayerNaught,
                    OwnerId = "swapwithloggedinuser"
                });
            return CustomGamesOk($"Game started for ID: {game.Id}",
                new GameDTO[] { game });
        }

        // PUT api/<GamesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CastTurnApiModel model)
        {
            GameDTO game = await _gameService.CastTurn(
                new TurnDto()
                {
                    Game = id,
                    Column = model.Column,
                    Row = model.Row,
                    CastedBy = "swapwithloggedinuser"
                });
            return CustomGamesOk($"Turn casted for game ID: {game.Id}",
                new GameDTO[] { game });      
        }

        // DELETE api/<GamesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new MethodAccessException();
        }

        private IActionResult CustomGamesOk(string message, IEnumerable<GameDTO> data)
        {
            return Ok(new SimpleOkApiModel
            {
                Message = message,
                ResultInfo = new
                {
                    count = data.Count(),
                    data
                }
            });
        }
    }
}
