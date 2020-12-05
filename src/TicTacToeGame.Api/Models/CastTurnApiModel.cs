using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToeGame.Api.Models
{
    public class CastTurnApiModel
    {
        [Range(1, 3, ErrorMessage = "Please enter a valid integer.")]
        public int Row { get; set; }
        [Range(1, 3, ErrorMessage = "Please enter a valid integer.")]
        public int Column { get; set; }
    }
}
