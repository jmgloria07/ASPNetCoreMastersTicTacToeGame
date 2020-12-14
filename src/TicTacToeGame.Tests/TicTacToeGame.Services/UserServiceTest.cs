using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TicTacToeGame.Services;
using Moq;
using Xunit;
using TicTacToeGame.Services.Utilities;
using TicTacToeGame.Api.Properties;
using Microsoft.Extensions.Options;
using TicTacToeGame.Services.Dto;

namespace TicTacToeGame.Tests.TicTacToeGame.Services
{
    public class UserServiceTest
    {
        //private readonly SignInManager<IdentityUser> _signinManager;
        private readonly IUserService _userService;
        private readonly JwtOptions _jwtOptions;
        private readonly TicTacToeHelper _ticTacToeHelper = new TicTacToeHelper();

        public UserServiceTest(IUserService userService,
            IOptions<JwtOptions> jwtOptions)
        {
            _userService = userService;
            _jwtOptions = jwtOptions.Value;
        }

        #region Tests
        [Fact]
        public async Task ConfirmEmailSuccess_Test()
        {
            var mockUser = this.GetMockUserData();
            var exception = await Record.ExceptionAsync(() => _userService.ConfirmEmail(mockUser.Id, _ticTacToeHelper.GenerateTokenAsync(mockUser, _jwtOptions.SecurityKey)));
            Assert.Null(exception);
        }

        [Fact]
        public async Task ConfirmEmailFail_Test()
        {
            var mockUser = this.GetMockUserData();
            var exception = await Record.ExceptionAsync(() => _userService.ConfirmEmail("", _ticTacToeHelper.GenerateTokenAsync(mockUser, _jwtOptions.SecurityKey)));
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task CreateLoginTokenFail_Test()
        {
            var mockUser = this.GetMockUserData();
            mockUser.Email = null;

            var exception = await Record.ExceptionAsync(() => _userService.CreateLoginToken(new UserDTO
            {
                Email = mockUser.Email,
                Password = "testpassword",
            }, _jwtOptions.SecurityKey));

            Assert.NotNull(exception);            
        }

        #endregion

        #region Mocks
        private IdentityUser GetMockUserData()
        {
            var user = new IdentityUser()
            {
                Id = "1234",
                UserName = "user@test.com"
            };
            return user;
        }

        public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            return mgr;
        }

        #endregion


    }
}
