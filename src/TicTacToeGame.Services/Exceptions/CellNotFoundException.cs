using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Exceptions
{
    public class CellNotFoundException : TicTacToeNotFoundException
    {
        public override string Message
        {
            get => "Cell not found.";
        }

        public override int TicTacToeErrorCode
        {
            get => 30001;
        }
    }
}
