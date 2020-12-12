using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToeGame.Services.Dto
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string EmailCode { get; set; }
        public string Password { get; set; }
        public string LoginToken { get; set; }
    }
}
