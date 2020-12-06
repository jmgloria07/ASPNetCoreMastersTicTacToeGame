using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Exceptions
{
    /*
     *  Specific for null parameters and should not happen live. Make sure
     *  to log when instances of this are caught outside the service 
     *  layer-- for the team to look into.
     */
    public class NullParameterException : TicTacToeLogicException
    {
        public override string Message
        {
            get => "Unhandled null parameter occurred.";
        }

        public override int TicTacToeErrorCode
        {
            get => 10001;
        }
    }
}
