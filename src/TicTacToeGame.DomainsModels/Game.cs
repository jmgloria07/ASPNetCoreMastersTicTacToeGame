using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicTacToeGame.DomainsModels
{
    public class Game
    {
        public enum State
        {
            CROSS_TURN = 1, 
            NAUGHT_TURN= 2, 
            CROSS_WON = 3, 
            NAUGHT_WON = 4,
            DRAW = 5
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public TicTacToe TicTacToe { get; set; }
        public string PlayerCross { get; set; }
        public string PlayerNaught { get; set; }
        public State GameState { get; set; }
        public string OwnerId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
