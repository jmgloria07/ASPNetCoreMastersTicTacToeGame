using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Dto
{
    public class TurnDto
    {
        public int Game { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string CastedBy { get; set; }
    }
}
