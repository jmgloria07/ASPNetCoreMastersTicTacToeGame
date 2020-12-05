using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Exceptions
{
    public abstract class TicTacToeServiceException : Exception
    {
        public override abstract string Message { get; }
    }
}
