using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TicTacToeGame.Services;
using Moq;
using Xunit;
using TicTacToeGame.Services.Utilities;
using TicTacToeGame.Api.Utilities;
using Microsoft.Extensions.Options;
using TicTacToeGame.Services.Dto;
using Microsoft.IdentityModel.Tokens;

namespace TicTacToeGame.Tests.TicTacToeGame.Services
{
    public class UserServiceTest
    {       
        private readonly TicTacToeHelper _ticTacToeHelper = new TicTacToeHelper();

        #region Tests
        [Fact]
        public async Task ConfirmEmailSuccess_Test()
        {
            var mockUser = this.GetMockUserData();
            IUserService _userService = MockConfirmEmail(mockUser);

            var exception = await Record.ExceptionAsync(() => _userService.ConfirmEmail(mockUser.Id, _ticTacToeHelper.GenerateTokenAsync(mockUser, GetMockSecurityKey())));
            Assert.Null(exception);
        }

        [Fact]
        public async Task CreateLoginTokenSuccess_Test()
        {
            var mockUser = this.GetMockUserData();
            IUserService _userService = MockCreateLoginToken();           

            var exception = await Record.ExceptionAsync(() => _userService.CreateLoginToken(new UserDTO
            {
                Email = mockUser.Email,
                Password = "testpassword",
            }, GetMockSecurityKey()));

            Assert.Null(exception);            
        }

        [Fact]
        public async Task RegisterWithConfirmationCodeSuccess_Test()
        {
            var mockUser = this.GetMockUserData();
            IUserService _userService = MockRegisterWithConfirmationCode();

            var exception = await Record.ExceptionAsync(() => _userService.RegisterWithConfirmationCode(new UserDTO
            {
                Email = mockUser.Email,
                Password = "testpassword",
            }));

            Assert.Null(exception);
        }
        #endregion

        #region Mocks
        private IdentityUser GetMockUserData()
        {
            var user = new IdentityUser()
            {
                Id = "1234",
                UserName = "user@test.com",
                Email = "user@test.com",
            };
            return user;
        }

        private SecurityKey GetMockSecurityKey()
        {
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4iLCJJc3N1ZXIiOiJJc3N1ZXIiLCJVc2VybmFtZSI6IlJleHh4IiwiZXhwIjoxNjA3OTY2NzE0LCJpYXQiOjE2MDc5NjY3MTR9.CQYLlvzak41WZeJ3THHCGXZS9103up3w6JzNMeeikfg"));
            return securityKey;
        }


        private IUserService MockConfirmEmail(IdentityUser userToMock)
        {
            Mock<IUserService> mockObject = new Mock<IUserService>();
            if (userToMock != null)
            {
                mockObject.Setup(m => m.ConfirmEmail(userToMock.Id, _ticTacToeHelper.GenerateTokenAsync(userToMock, this.GetMockSecurityKey())));
            } 
            return mockObject.Object;
        }

        private IUserService MockCreateLoginToken()
        {
            var mockUser = this.GetMockUserData();
            Mock<IUserService> mockObject = new Mock<IUserService>();
            if (mockUser != null)
            {
                mockObject.Setup(m => m.CreateLoginToken(new UserDTO
                {
                    Email = mockUser.Email,
                    Password = "testpassword",
                }, GetMockSecurityKey()));
            }
            return mockObject.Object;
        }

        private IUserService MockRegisterWithConfirmationCode()
        {
            var mockUser = this.GetMockUserData();
            Mock<IUserService> mockObject = new Mock<IUserService>();
            if (mockUser != null)
            {
                mockObject.Setup(m => m.RegisterWithConfirmationCode(new UserDTO
                {
                    Email = mockUser.Email,
                    Password = "testpassword",
                }));
            }
            return mockObject.Object;
        }

        #endregion
    }
}
