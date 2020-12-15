using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using TicTacToeGame.Api.Models;
using TicTacToeGame.Api.Properties;
using TicTacToeGame.Services;
using TicTacToeGame.Services.Dto;
using TicTacToeGame.Services.Interfaces.Services;

namespace TicTacToeGame.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly JwtOptions _jwtOptions;

        private const string EMAIL_CONFIRMED_TXT = "Email successfully confirmed.";
        private const string EMAIL_CONFIRM_SUBJECT = "Email Confirmation!";
        private const string PLEASE_CONFIRM_MSG = "Please check your inbox to confirm the registered email.";
        private const string LOGIN_SUCCESS_TXT = "Login is successful.";

        public UsersController(IUserService userService,
            IEmailService emailService,
            IOptions<JwtOptions> jwtOptions)
        {
            _userService = userService;
            _emailService = emailService;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost]
        public async Task<ActionResult<AbstractResponseApiModel>> Register([FromBody] RegisterApiModel form)
        {
            UserDTO user = new UserDTO
            {
                Email = form.Email,
                Password = form.Password
            };

            user = await _userService.RegisterWithConfirmationCode(user);
            
            string encodedCode = WebEncoders.Base64UrlEncode(
                    Encoding.UTF8.GetBytes(user.EmailCode));

            //TODO: convert the following to a proper url object. Currently
            //using Request to construct the URL. This may become a security concern.
            string confirmEmailUrl = Request.Scheme + "://"
                + Request.Host
                + $"/api/users/{user.Id}/email/confirm?code={encodedCode}";

            var message = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmEmailUrl)}'>clicking here</a>.";

            await _emailService.SendEmailAsync(user.Email, EMAIL_CONFIRM_SUBJECT, message);

            //TODO: send the generated URL to email
            return Accepted("/users/{user.Id}", new OkResponseApiModel
            {
                Message = PLEASE_CONFIRM_MSG,
                ResultInfo = new
                {
                    note = "This data should be temporary.",
                    confirmEmailUrl
                }
            });
        }

        [HttpGet("{userId}/email/confirm")]
        public async Task<ActionResult<AbstractResponseApiModel>> ConfirmEmail(string userId, [FromQuery] string code)
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            await _userService.ConfirmEmail(userId, code);

            return Created("/users/{user.Id}", new OkResponseApiModel
            {
                Message = EMAIL_CONFIRMED_TXT
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AbstractResponseApiModel>> Login([FromBody] LoginApiModel model)
        {
            UserDTO user = await _userService.CreateLoginToken(new UserDTO
            {
                Email = model.Email,
                Password = model.Password
            }, _jwtOptions.SecurityKey);

            return Ok(new OkResponseApiModel
            {
                Message = LOGIN_SUCCESS_TXT,
                ResultInfo = new
                {
                    id = user.Id,
                    token = user.LoginToken
                }
            });
        }
    }
}
