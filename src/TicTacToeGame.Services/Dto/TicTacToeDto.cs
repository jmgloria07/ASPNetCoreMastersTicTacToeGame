using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Dto
{
    public class TicTacToeDto
    {
        public IEnumerable<CellDto> Cells { get; set; }
    }
}
