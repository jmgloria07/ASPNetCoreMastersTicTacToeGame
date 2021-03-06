﻿using System;
using TicTacToeGame.Services.Dto;
using TicTacToeGame.Services.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TicTacToeGame.Services.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace TicTacToeGame.Services
{
    public class UserService : IUserService
    {
        private readonly TicTacToeHelper _ticTacToeHelper = new TicTacToeHelper();
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(SignInManager<IdentityUser> signinManager,
            UserManager<IdentityUser> userManager)
        {
            _signinManager = signinManager;
            _userManager = userManager;
        }

        public async Task ConfirmEmail(string userId, string code)
        {
            var identityUser = await _userManager.FindByIdAsync(userId);
            if (identityUser == null) throw new UserNotFoundException();

            var result = await _userManager.ConfirmEmailAsync(identityUser, code);

            if (!result.Succeeded) throw new EmailCodeDoesNotMatchException();

        }
        public async Task<UserDTO> CreateLoginToken(UserDTO user, SecurityKey securityKey)
        {
            var result = await _signinManager.PasswordSignInAsync(user.Email, 
                    user.Password, 
                    isPersistent: false, 
                    lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                throw new UserLoginException();
            }
            var identityUser = await _userManager.FindByEmailAsync(user.Email);
            return new UserDTO
            {
                Id = identityUser.Id,
                LoginToken = _ticTacToeHelper.GenerateTokenAsync(identityUser, securityKey)
            };
        }

        public async Task<UserDTO> RegisterWithConfirmationCode(UserDTO user)
        {
            var identityUser = new IdentityUser { 
                UserName = user.Email, 
                Email = user.Email 
            };
            var result = await _userManager.CreateAsync(identityUser, user.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    bool isPasswordRelated = error.Code.Contains("Password");
                    if (isPasswordRelated) throw new InvalidParameterException();
                }
                throw new UserAlreadyExistsException();
            }
            
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);

            return new UserDTO
            {
                Id = identityUser.Id,
                Email = identityUser.Email,
                EmailCode = code
            };
        }       
    }
}
