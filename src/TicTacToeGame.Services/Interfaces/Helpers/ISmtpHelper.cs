using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TicTacToeGame.Services.Utilities;

namespace TicTacToeGame.Services.Interfaces.Helpers
{
    public interface ISmtpHelper
    {
        /// <summary>
        /// Gets or sets the email sender.
        /// </summary>
        /// <value>
        /// The sender.
        /// </value>
        string Sender { get; set; }

        /// <summary>
        /// Gets or sets the reply-to email.
        /// </summary>
        /// <value>
        /// The reply to.
        /// </value>
        List<string> ReplyTo { get; set; }

        /// <summary>
        /// Gets or sets the subject of the email.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        string Subject { get; set; }

        /// <summary>
        /// Gets or sets the list of recipients.
        /// </summary>
        /// <value>
        /// To recipients.
        /// </value>
        List<string> ToRecipients { get; set; }

        /// <summary>
        /// Gets or sets the list of CC recipients.
        /// </summary>
        /// <value>
        /// The cc recipients.
        /// </value>
        List<string> CcRecipients { get; set; }

        /// <summary>
        /// Gets or sets the list of BCC recipients.
        /// </summary>
        /// <value>
        /// The BCC recipients.
        /// </value>
        List<string> BccRecipients { get; set; }

        /// <summary>
        /// Gets or sets the email content.
        /// </summary>
        /// <value>
        /// The email body.
        /// </value>
        string EmailBody { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether email content is to be rendered in HTML.
        /// </summary>
        /// <value>
        ///   <c>true</c> if email content is to be rendered in HTML; otherwise, <c>false</c>.
        /// </value>
        bool IsHtml { get; set; }

        /// <summary>
        /// Gets or sets the SMTP settings.
        /// </summary>
        /// <value>
        /// The SMTP settings.
        /// </value>
        SmtpSettings SmtpSettings { get; set; }

        /// <summary>
        /// Sends the email
        /// </summary>
        /// <returns>
        /// Returns true if email sending is successful; otherwise, returns false
        /// </returns>
        Task Send();

        /// <summary>
        /// Sends the email with attachment
        /// </summary>
        /// <param name="attachments"></param>
        /// <returns>
        /// Returns true if email sending is successful; otherwise, returns false
        /// </returns>
        Task Send(ArrayList attachments);
    }
}
