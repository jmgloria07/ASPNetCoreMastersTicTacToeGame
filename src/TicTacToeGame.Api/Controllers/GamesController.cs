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

namespace TicTacToeGame.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GamesController : ControllerBase
    {
        private readonly IAuthorizationService _authService;
        private readonly IGameService _gameService;

        public GamesController(IAuthorizationService authService, 
            IGameService gameService)
        {
            _authService = authService;
            _gameService = gameService;
        }

        // GET: api/<GamesController>
        [HttpGet]
        public async Task<ActionResult<AbstractResponseApiModel>> Get()
        {
            IEnumerable<GameDTO> games = _gameService.GetGames();

            games = await FilterForbiddenGames(games);

            if (games == null || games.Count() == 0)
                throw new TicTacToeNotFoundException();

            return CustomOk(
                $"Found {games.Count()} games.", 
                games);
        }

        // GET api/<GamesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AbstractResponseApiModel>> Get(int id)
        {
            GameDTO game = await _gameService.GetGame(id);

            if (game == null || await IsSingleGameForbidden(game))
                throw new TicTacToeNotFoundException();

            return CustomOk(
                    $"Found game with ID: {id}; {game.GameState}", 
                    new GameDTO[] { game });
        }

        // POST api/<GamesController>
        [HttpPost]
        public async Task<ActionResult<AbstractResponseApiModel>> Post([FromBody] CreateGameApiModel model)
        {
            GameDTO game = await _gameService.Save(
                new GameDTO()
                {
                    PlayerCross = model.PlayerCross,
                    PlayerNaught = model.PlayerNaught,
                    OwnerId = User.FindFirst("Id").Value
                });
            return CustomOk(
                    $"Game started for ID: {game.Id}",
                    new GameDTO[] { game });
        }

        // PUT api/<GamesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AbstractResponseApiModel>> Put(int id, [FromBody] CastTurnApiModel model)
        {
            GameDTO game = await _gameService.CastTurn(
                new TurnDto()
                {
                    Game = id,
                    Column = model.Column,
                    Row = model.Row,
                    CastedBy = User.FindFirst("Id").Value
                });
            return CustomOk(
                    $"Turn casted for game ID: {game.Id}",
                    new GameDTO[] { game });      
        }

        // DELETE api/<GamesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }


        //-------- private methods ------------//
        private ActionResult<AbstractResponseApiModel> CustomOk(string message, IEnumerable<GameDTO> data)
        {
            return Ok(new OkResponseApiModel
            {
                Message = message,
                ResultInfo = new
                {
                    count = data.Count(),
                    data
                }
            });
        }

        private async Task<IEnumerable<GameDTO>> FilterForbiddenGames(IEnumerable<GameDTO> games)
        {
            if (games == null) return games;

            foreach (var itemRetrieved in games)
            {
                if (await IsSingleGameForbidden(itemRetrieved))
                {
                    games = games.Where(item => item.Id != itemRetrieved.Id);
                }
            }

            return games;
        }

        private async Task<bool> IsSingleGameForbidden(GameDTO game)
        {
            var authResult = await _authService.AuthorizeAsync(User, game, "UserShouldBeInvolved");
            return !authResult.Succeeded;
        }
    }
}
