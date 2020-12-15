using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame.Services.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipient, string subject, string htmlMessage);
        Task SendEmailAsync(List<string> recipients, string subject, string htmlMessage);
    }
}
