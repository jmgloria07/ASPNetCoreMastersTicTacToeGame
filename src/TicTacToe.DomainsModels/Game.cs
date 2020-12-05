using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicTacToe.DomainsModels
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public TicTacToe TicTacToe { get; set; }
        public string PlayerOne { get; set; }
        public string PlayerTwo { get; set; }
        public GameState State { get; set; }
        public string OwnerId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
