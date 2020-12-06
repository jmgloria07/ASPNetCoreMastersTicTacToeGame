using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Exceptions
{
    public class TurnUncastedException : TicTacToeIdentityException
    {
        public override string Message
        {
            get => "Unable to cast turn. Please ensure if this is your turn.";
        }

        public override int TicTacToeErrorCode
        {
            get => 21003;
        }
    }
}
