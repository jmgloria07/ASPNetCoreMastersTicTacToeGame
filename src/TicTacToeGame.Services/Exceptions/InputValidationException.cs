using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Exceptions
{
    public class InputValidationException : TicTacToeServiceException
    {
        public override string Message { 
            get => "Please check your input."; 
        }
    }
}
