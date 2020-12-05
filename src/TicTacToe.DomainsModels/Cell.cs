using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TicTacToe.DomainsModels
{
    public class Cell
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public CellState State { get; set; }
        public string OwnerId { get; set; }
        public DateTime SetDate { get; set; }
    }
}
