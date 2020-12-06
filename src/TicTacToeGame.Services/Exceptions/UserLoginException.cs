using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Exceptions
{
    public class UserLoginException : TicTacToeIdentityException
    {
        public override string Message
        {
            get => "Login failed. Please retry.";
        }

        public override int TicTacToeErrorCode
        {
            get => 20002;
        }
    }
}
