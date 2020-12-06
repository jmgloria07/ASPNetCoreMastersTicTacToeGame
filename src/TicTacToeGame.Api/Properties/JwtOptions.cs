using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToeGame.Api.Properties
{
    public class JwtOptions
    {
        public SecurityKey SecurityKey { get; set; }
    }
}
