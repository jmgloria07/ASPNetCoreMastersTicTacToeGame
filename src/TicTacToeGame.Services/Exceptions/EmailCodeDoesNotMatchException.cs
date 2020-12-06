using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Exceptions
{
    public class EmailCodeDoesNotMatchException : TicTacToeIdentityException
    {
        public override string Message
        {
            get => "Email code is invalid. Please check your inbox to confirm the registered email.";
        }
        public override int TicTacToeErrorCode
        {
            get => 20004;
        }
    }
}
