using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Exceptions
{
    /* 
     *  Simple business logic exception class. Please specify 
     *  as much as possible
     */
    public class TicTacToeLogicException : TicTacToeServiceException
    {
        public override string Message 
        { 
            get => "Please check your input."; 
        }

        public override int TicTacToeErrorCode
        {
            get => 10000;
        }
    }
}
