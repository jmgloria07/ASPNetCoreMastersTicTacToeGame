using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Exceptions
{
    public class UserNotFoundException : TicTacToeIdentityException
    {
        public override string Message
        {
            get => "User not found.";
        }

        public override int TicTacToeErrorCode
        {
            get => 20005;
        }
    }
}
