using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToeGame.Api.Models
{
    public class ErrorResultInfo
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
