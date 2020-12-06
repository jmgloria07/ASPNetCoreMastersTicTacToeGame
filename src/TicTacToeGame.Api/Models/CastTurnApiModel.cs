using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToeGame.Api.Models
{
    public class CastTurnApiModel
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
