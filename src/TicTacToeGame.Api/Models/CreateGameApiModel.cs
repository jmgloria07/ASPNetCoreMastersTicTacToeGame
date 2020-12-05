using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToeGame.Api.Models
{
    public class CreateGameApiModel
    {
        [Required]
        public string PlayerCross { get; set; }
        [Required]
        public string PlayerNaught { get; set; }
    }
}
