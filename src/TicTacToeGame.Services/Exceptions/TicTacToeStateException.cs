using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Exceptions
{
    public class TicTacToeStateException : TicTacToeLogicException
    {
        private readonly object _obj;
        public TicTacToeStateException(object obj)
        {
            _obj = obj;
        }
        public override string Message
        {
            get => "State is invalid: " + _obj.ToString();
        }

        public override int TicTacToeErrorCode
        {
            get => 10002;
        }
    }
}
