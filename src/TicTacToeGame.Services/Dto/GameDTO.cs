using TicTacToeGame.DomainsModels;

namespace TicTacToeGame.Services.Dto
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string PlayerCross { get; set; }
        public string PlayerNaught { get; set; }
        public TicTacToeDto TicTacToe { get; set; }
        public Game.State GameState { get; set; }
        public string OwnerId { get; set; }
    }
}