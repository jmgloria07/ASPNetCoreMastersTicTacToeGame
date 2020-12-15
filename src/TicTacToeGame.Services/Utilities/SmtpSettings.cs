using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeGame.Services.Utilities
{
    public class SmtpSettings
    {
        public string SmtpHost { get; set; }

        public int? SmtpPort { get; set; }

        public string SmtpUserName { get; set; }

        public string SmtpPassword { get; set; }
    }
}
