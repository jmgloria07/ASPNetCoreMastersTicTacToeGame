using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToeGame.Services.Dto;
using Microsoft.IdentityModel.Tokens;

namespace TicTacToeGame.Services
{
    public interface IUserService
    {
        public Task<UserDTO> RegisterWithConfirmationCode(UserDTO user);
        public Task<string> CreateLoginToken(UserDTO user, SecurityKey securityKey);
        public Task ConfirmEmail(string userId, string code);
    }
}
