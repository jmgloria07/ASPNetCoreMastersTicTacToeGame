using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Exceptions
{
    public class UserAlreadyExistsException : TicTacToeIdentityException
    {
        public override string Message
        {
            get => "Failed to register; User may already be existing.";
        }

        public override int TicTacToeErrorCode
        {
            get => 20001;
        }
    }
}
