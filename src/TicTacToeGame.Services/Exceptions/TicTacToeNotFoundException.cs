using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Exceptions
{
    public class TicTacToeNotFoundException : TicTacToeServiceException
    {
        public override string Message => "No games were found. Please create one.";

        public override int TicTacToeErrorCode => 30000;
    }
}
