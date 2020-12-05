using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public ActionResult Get()
        {
            IEnumerable<GameDTO> games = _gameService.GetGames();
            if (games == null) return NotFound();
            return Ok(games);
        }

        // GET api/<GamesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            GameDTO game = await _gameService.GetGame(id);
            if (game == null) return NotFound();
            return Ok(game);
        }

        // POST api/<GamesController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateGameApiModel model)
        {
            return Ok(await _gameService.Save(
                new GameDTO()
                {
                    PlayerCross = model.PlayerCross,
                    PlayerNaught = model.PlayerNaught,
                    OwnerId = "swapwithloggedinuser"
                })
            );
        }

        // PUT api/<GamesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CastTurnApiModel model)
        {
            try
            {
                return Ok(await _gameService.CastTurn(
                new TurnDto()
                {
                    Game = id,
                    Column = model.Column,
                    Row = model.Row,
                    CastedBy = "swapwithloggedinuser"
                }));
            }
            catch (InputValidationException ex)
            {
                return BadRequest(new { 
                    ex.Message,
                    ex.HelpLink,
                    Date = DateTime.Now
                });
            }
            
        }

        // DELETE api/<GamesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
