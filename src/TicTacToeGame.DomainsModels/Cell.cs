using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TicTacToeGame.DomainsModels
{
    public class Cell
    {
        public enum State
        {
            BLANK, CROSS, NAUGHT
        }
        public enum Row
        {
            ONE = 1, TWO = 2, THREE = 3
        }
        public enum Column
        {
            A = 1, B = 2, C = 3
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[JsonIgnore]
        public int Id { get; set; }
        public Row RowNum { get; set; }
        public Column ColumnField { get; set; }
        public State CellState { get; set; }
        public string OwnerId { get; set; }
        public string CastedBy { get; set; }
        public DateTime SetDate { get; set; }
    }
}
