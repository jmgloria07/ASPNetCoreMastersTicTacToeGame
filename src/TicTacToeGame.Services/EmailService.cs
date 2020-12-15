using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TicTacToeGame.Configurations;
using TicTacToeGame.Services.Interfaces.Helpers;
using TicTacToeGame.Services.Interfaces.Services;
using TicTacToeGame.Services.Utilities;

namespace TicTacToeGame.Services
{
    public class EmailService : IEmailService
    {
        private readonly ISmtpHelper _smtpHelper;
        private readonly SmtpConfiguration _smtpConfiguration;
        private const string GAME_MASTER_EMAIL = "gamemaster@magenic.com";

        public EmailService(ISmtpHelper smtpHelper, 
            IOptions<SmtpConfiguration> smtpConfiguration)
        {
            _smtpHelper = smtpHelper;
            _smtpConfiguration = smtpConfiguration.Value;
        }

        public async Task SendEmailAsync(string recipient, string subject, string htmlMessage)
        {
            await SendEmailAsync(new List<string>() { recipient }, subject, htmlMessage);
        }

        public async Task SendEmailAsync(List<string> recipients, string subject, string htmlMessage)
        {
            _smtpHelper.Subject = subject;
            _smtpHelper.Sender = GAME_MASTER_EMAIL;
            _smtpHelper.ToRecipients = recipients;
            _smtpHelper.EmailBody = htmlMessage;
            _smtpHelper.SmtpSettings = new SmtpSettings
            {
                SmtpHost = _smtpConfiguration.SmtpHost,
                SmtpPassword = _smtpConfiguration.SmtpPassword,
                SmtpPort = _smtpConfiguration.SmtpPort,
                SmtpUserName = _smtpConfiguration.SmtpUserName
            };

            await _smtpHelper.Send();
        }
    }
}
