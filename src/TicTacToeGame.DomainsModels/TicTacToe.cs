using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TicTacToeGame.DomainsModels
{
    public class TicTacToe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public IEnumerable<Cell> Cells { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
