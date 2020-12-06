using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Exceptions
{
    /* 
     *  Simple user authentication exception class. Please specify 
     *  as much as possible
     */
    public class TicTacToeIdentityException : TicTacToeServiceException
    {
        public override string Message
        {
            get => "Invalid User Credentials.";
        }

        public override int TicTacToeErrorCode
        {
            get => 20000;
        }
    }
}
