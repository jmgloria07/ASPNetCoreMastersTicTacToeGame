using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TicTacToe.DomainsModels
{
    public class TicTacToe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Cell A1 { get; set; }
        public Cell A2 { get; set; }
        public Cell A3 { get; set; }
        public Cell B1 { get; set; }
        public Cell B2 { get; set; }
        public Cell B3 { get; set; }
        public Cell C1 { get; set; }
        public Cell C2 { get; set; }
        public Cell C3 { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
