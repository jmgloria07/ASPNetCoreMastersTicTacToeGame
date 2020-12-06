using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Exceptions
{
    /*  
     *  Abstract application level exception class, for handling
     *  exceptions thrown at the service level.
     */
    public abstract class TicTacToeServiceException : Exception
    {
        public override abstract string Message { get; }
        public abstract int TicTacToeErrorCode { get; }
    }
}
