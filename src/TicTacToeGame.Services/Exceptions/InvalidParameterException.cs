using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Exceptions
{
    public class InvalidParameterException : TicTacToeLogicException
    {
        public override int TicTacToeErrorCode
        {
            get => 10003;
        }
    }
}
