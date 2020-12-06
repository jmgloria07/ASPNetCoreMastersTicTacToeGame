using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToeGame.Api.Models
{
    public class AbstractResponseApiModel
    {
        public string Message { get; set; }
        public object ResultInfo { get; set; }
    }
}
